﻿using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.Helpers
{
    public class OfferViewModelInitializer
    {
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly IConditionGetterService _conditionGetterService;
        private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
        public OfferViewModelInitializer(ICategoryGetterService categoryGetterService,
            IConditionGetterService conditionGetterService,
            IDeliveryTypeGetterService deliveryTypeGetterService)
        {
            _categoryGetterService = categoryGetterService;
            _conditionGetterService = conditionGetterService;
            _deliveryTypeGetterService = deliveryTypeGetterService;
        }
        public async Task InitializeAllAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseOfferViewModel
        {
            await PopulateOtherDeliveryTypes<TViewModel>(viewModel);
            await PopulateParcelLockerDeliveries<TViewModel>(viewModel);
            await PopulateCategories<TViewModel>(viewModel);
            await PopulateConditions<TViewModel>(viewModel);
        }
        public async Task PopulateParcelLockerDeliveries<TViewModel>(TViewModel viewModel) where TViewModel : BaseOfferViewModel
        {
            viewModel.ParcelLockerDeliveriesList =
                (await _deliveryTypeGetterService.GetParcelLockerDeliveryTypes()).ConvertToDeliveryTypeViewModelList();
        }

        public async Task PopulateConditions<TViewModel>(TViewModel viewModel) where TViewModel : BaseOfferViewModel
        {
            viewModel.ProductConditionsSelectList =
                (await _conditionGetterService.GetProductConditionsAsSelectList()).ConvertToSelectListItem();
        }

        public async Task PopulateCategories<TViewModel>(TViewModel viewModel) where TViewModel : BaseOfferViewModel
        {
            viewModel.ProductCategoriesSelectionList =
                (await _categoryGetterService.GetProductCategoriesAsSelectList()).ConvertToSelectListItem();
        }

        public async Task PopulateOtherDeliveryTypes<TViewModel>(TViewModel viewModel) where TViewModel : BaseOfferViewModel
        {
            viewModel.OtherDeliveriesSelectedList =
                (await _deliveryTypeGetterService.GetOtherDeliveryTypes()).ConvertToSelectListItem();
        }

    }
}
