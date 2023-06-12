using Core.DAL.Infrastructure;
using Core.Library.Base;
using IPSBlazor.Services;
using IPSTemplate.UI.Blazor.Helpers;
using Microsoft.Extensions.Localization;
using System.Reflection;
using Telerik.Blazor.Components;

namespace IPSTemplate.UI.Blazor.Base;

public abstract class GridView<TSelf, TInfo, TGridInfo, TReadOnlyList, TEntity> : ComponentBase
    where TSelf : GridView<TSelf, TInfo, TGridInfo, TReadOnlyList, TEntity>
    where TInfo : CslaReadOnlyBase<TInfo, TEntity>
    where TGridInfo : CslaReadOnlyGridBase<TGridInfo, TReadOnlyList, TEntity, TReadOnlyList, TInfo>
    where TReadOnlyList : CslaReadOnlyListBase<TReadOnlyList, TInfo, TEntity>
    where TEntity : Entity
{
    private MobileCslaRequest? _lastRequest;
    protected TelerikGrid<TInfo>? Grid { get; set; } = default!;

    /// <summary>
    /// Default grid page sizes: 10, 20, 50, 100, 200
    /// </summary>
    protected virtual List<int?> GridPageSizes { get; set; } = new() { 10, 20, 50, 100, 200 };
    protected IEnumerable<TInfo> SelectedItems { get; set; } = Enumerable.Empty<TInfo>();
    protected int Page { get; set; }
    protected int PageSize { get; set; }
    protected int TotalRowCount { get; set; }
    protected bool Exporting { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] protected IStringLocalizer<Properties.Resources> Localizer { get; set; } = default!;
    [Inject] protected IDataPortalFactory DataPortalFactory { get; set; } = default!;
    [Inject] protected IDataPortal<MobileCslaRequest> MobileRequestPortal { get; set; } = default!;
    [Inject] protected IDataPortal<CslaRequestSort> RequestSortPortal { get; set; } = default!;
    [Inject] protected IDataPortal<CslaRequestFilter> RequestFilterPortal { get; set; } = default!;

    protected override void OnInitialized()
    {
        Page = NavigationManager.TryGetQueryParameter("page", out int page) ? page : 1;
        PageSize = NavigationManager.TryGetQueryParameter("pageSize", out int pageSize) ? pageSize : 20;
    }

    protected abstract Task<TGridInfo> GetGridData(MobileCslaRequest request);

    protected async Task ReadData(ReadEventArgs args)
    {
        var request = await args.Request.ConvertToCslaRequestAsync(MobileRequestPortal, RequestSortPortal, RequestFilterPortal);
        var data = await GetGridData(request);
        args.Data = data.Data;
        args.Total = data.TotalRowCount;
        TotalRowCount = data.TotalRowCount;
        StateHasChanged();
    }

    [Obsolete("Use the ReadData method with the IPSGrid's OnRead event instead")]
    protected async Task<(IEnumerable<TInfo> data, int totalCount)> ReadDataHandler(MobileCslaRequest request)
    {
        _lastRequest = request;
        var data = await GetGridData(request);
        TotalRowCount = data.TotalRowCount;
        StateHasChanged();

        if (request.Page > 1)
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, new Dictionary<string, string?>
            {
                { "page", Page.ToString() },
                { "pageSize", PageSize.ToString() },
            });
        }

        return (data.Data, data.TotalRowCount);
    }

    private async Task DeleteItem(Func<Task<Csla.IBusinessBase>> editableObjectFactory)
    {
        var actionConfirmed = await DialogService.ShowConfirmAsync(
            message: Localizer["GridView_DeleteConfirmMessage"],
            title: Localizer["GridView_DeleteConfirmTitle"],
            confirmActionText: Localizer["GridView_DeleteConfirmConfirmText"],
            cancelActionText: Localizer["GridView_DeleteConfirmCancelText"]);

        if (!actionConfirmed) return;

        var editable = await editableObjectFactory();
        editable.Delete();
        await editable.SaveAsync();
    }

    public void Refresh()
    {
        Grid?.Rebind();
    }

    protected async Task ExportToExcel()
    {
        Exporting = true;

        var request = _lastRequest;
        if (request is null) return;

        request.Page = 1;
        request.PageSize = int.MaxValue;

        Type modelType = typeof(TReadOnlyList);
        var factoryMethod = modelType.GetMethod(nameof(CslaReadOnlyListBase<TReadOnlyList, TInfo, TEntity>.GetListAsync));
        if (factoryMethod is null && modelType.BaseType == typeof(CslaReadOnlyListBase<TReadOnlyList, TInfo, TEntity>))
        {
            factoryMethod = modelType.BaseType.GetMethod(nameof(CslaReadOnlyListBase<TReadOnlyList, TInfo, TEntity>.GetListAsync));
        }

        var task = (Task<TReadOnlyList>?)factoryMethod?.Invoke(null, new object?[] { request });
        if (task is null) return;

        TReadOnlyList data = await task;

        //StringBuilder sb = new StringBuilder();
        //sb.AppendLine(DIOrderInfo.GetCsvHeaderRow());
        //foreach (var item in data)
        //{
        //    sb.AppendLine(item.GetCsvRow());
        //}

        //await FileExporter.Save(
        //    jsRuntime,
        //    Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(),
        //    "application/csv",
        //    "Instances.csv");

        Exporting = false;
        NotificationService.ShowSuccess(Localizer["Global_ExportCompleted"]);
    }

    private static System.Reflection.MethodInfo? GetFactoryMethod(Type modelType, string methodName)
    {
        return modelType.GetMethod(methodName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
    }
}
