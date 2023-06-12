using Csla;
using Csla.Blazor;
using IPSBlazor.Services;
using Microsoft.Extensions.Localization;
using Resources = IPSTemplate.UI.Blazor.Properties.Resources;

namespace IPSTemplate.UI.Blazor;

internal static class ViewModelExtensions
{
    public static async Task<bool> IsCleanAsync<T>(this ViewModel<T> viewModel, DialogService? dialogService, IStringLocalizer localizer)
        where T : IBusinessBase
    {
        if (viewModel.Model.IsSavable)
        {
            if (dialogService is null)
            {
                return false;
            }

            var shouldContinue = await dialogService.ShowConfirmAsync(
                localizer[Resources.SaveChangesDialog_Message],
                localizer[Resources.SaveChangesDialog_Title],
                localizer[Resources.SaveChangesDialog_ConfirmActionText],
                localizer[Resources.SaveChangesDialog_CancelActionText]);

            if (!shouldContinue)
            {
                return false;
            }

            await viewModel.SaveAsync();
            return !viewModel.Model.IsSavable;
        }

        if (!viewModel.Model.IsValid)
        {
            if (dialogService is not null)
            {
                await dialogService.ShowAlertAsync(
                    localizer[Resources.CheckAndEditDataDialog_Message],
                    localizer[Resources.CheckAndEditDataDialog_Title]);
            }

            return false;
        }

        return true;
    }
}
