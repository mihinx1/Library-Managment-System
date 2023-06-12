using System.ComponentModel;
using IPSBlazor.Models.Generic;

namespace IPSTemplate.UI.Blazor.Base;

public abstract class EditGroup<TModel, TViewModel> : ComponentBase, IDisposable
    where TModel : Csla.Core.BindableBase
{
    private CancellationTokenSource _cts = new();

    [Parameter] public virtual string? Heading { get; set; }

    [Parameter] public IViewModelProperty<TModel> Model { get; set; } = default!;

    [CascadingParameter] protected ViewModel<TViewModel> ViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Model is null) throw new ArgumentNullException(nameof(Model));
        if (ViewModel is null) throw new ArgumentNullException(nameof(ViewModel));

        ViewModel.ModelPropertyChanged += HandleModelPropertyChanged;

        base.OnInitialized();
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
