using System.ComponentModel;
using System.Reflection;
using Core.DAL.Infrastructure;
using Core.Library.Base;
using IPSBlazor.Services;

namespace IPSTemplate.UI.Blazor.Base;

public partial class EditView<TModel, TEntity> : PageBase, IDisposable
    where TModel : CslaBusinessBase<TModel, TEntity>
    where TEntity : Entity
{
    private CancellationTokenSource _cts = new();

    protected string? Error { get; set; }

    protected bool Saving { get; set; }

    [Inject] public ViewModel<TModel> ViewModel { get; protected set; } = default!;

    [Inject] protected NotificationService NotificationService { get; set; } = default!;

    [Inject] protected DialogService DialogService { get; set; } = default!;

    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject] protected IDataPortalFactory DataPortalFactory { get; set; } = default!;

    [Parameter] public string? ItemId { get; set; }

    [Parameter] public TModel? Item { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        ViewModel.ModelPropertyChanged += HandleModelPropertyChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        Task<TModel>? modelTask = null;
        MethodInfo? factoryMethod;

        if (Item != null)
        {
            modelTask = Task.FromResult(Item);
        }
        else if (string.IsNullOrEmpty(ItemId))
        {
            factoryMethod = GetFactoryMethod(typeof(TModel), nameof(CslaBusinessBase<TModel, TEntity>.NewItemAsync));
            modelTask = (Task<TModel>?)factoryMethod?.Invoke(null, new object[] { DataPortalFactory });
        }
        else if (Guid.TryParse(ItemId, out var result) && result != Guid.Empty)
        {
            factoryMethod = GetFactoryMethod(typeof(TModel), nameof(CslaBusinessBase<TModel, TEntity>.GetItemAsync));
            modelTask = (Task<TModel>?)factoryMethod?.Invoke(null, new object[] { result, DataPortalFactory });
        }
        else if (int.TryParse(ItemId, out var intResult) && intResult > 0) //for loading DILog
        {
            factoryMethod = GetFactoryMethod(typeof(TModel), nameof(CslaBusinessBase<TModel, TEntity>.GetItemAsync));
            modelTask = (Task<TModel>?)factoryMethod?.Invoke(null, new object[] { intResult, DataPortalFactory });
        }

        await ViewModel.RefreshAsync(() => modelTask);

        if (ViewModel.Model is null)
        {
            Error = Localizer["Global_DataNotFound"];
        }

        await base.OnInitializedAsync();
    }

    protected async Task SafeCancel(string redirectUrl)
    {
        bool shouldCancel = !ViewModel.Model.IsDirty || await DialogService.ShowLeavePageConfirm();
        if (shouldCancel)
        {
            NavigationManager.NavigateTo(redirectUrl);
        }
    }

    protected async Task DelayedStateChange()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        await Task.Delay(200, _cts.Token).ContinueWith(async (x) =>
        {
            if (x.IsCompletedSuccessfully)
            {
                await InvokeAsync(StateHasChanged);
            }
        });
    }

    private static MethodInfo? GetFactoryMethod(Type modelType, string methodName)
    {
        return modelType.GetMethod(methodName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
    }

    private async void HandleModelPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        await DelayedStateChange();
    }

    public void Dispose()
    {
        ViewModel.ModelPropertyChanged -= HandleModelPropertyChanged;
        GC.SuppressFinalize(this);
    }
}
