(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalBankService = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'ProductName';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalBankService.prototype.init = function () {
        module._APortalBankService.load();
    }

    _APortalBankService.prototype.load = function () {
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
        APortalBankService._APortalBankService.List();
    }

    _APortalBankService.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_LoanProductMaster'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalBankService._APortalBankService._pageIndex > 0) {
                $(`${_root} #BankService-tbody-render`).append(response);
            } else if (APortalBankService._APortalBankService._pageIndex == 0) {
                $(`${_root} #BankService-tbody-render`).empty();
                $(`${_root} #BankService-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblBankServiceList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblBankServiceList tbody tr`).length ? $(`${_root} #tblBankServiceList tbody tr`).length : PageTotal;
            APortalBankService._APortalBankService._pageIndex = $(`${_root} #tblBankServiceList tbody tr`).length;
            APortalBankService._APortalBankService._totalRecords = parseInt($(`${_root} #tblBankServiceList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalBankService.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_LoanProductMaster' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_BankService_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_BankService_form').reset();
                document.querySelectorAll('#nav_BankService_form input, #nav_BankService_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(`${_root} #h3_addBankServiceHeader`).empty().html('Add New Service Details');
            $(document).ready(function () {
                $('.select2').select2({
                    placeholder: "-- Select --",
                    allowClear: true,
                    width: '100%'
                });
            });
            $(`${_root} #BankService-Master-Panel`).toggleClass('show');
        });
    }

    _APortalBankService.prototype.SubmitFormData = function (event, form) {
        $("#nav_BankService_form").removeData("validator");
        $("#nav_BankService_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_BankService_form"));
        event.preventDefault();
        if ($("#nav_BankService_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponsewithFile(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_BankService_form').reset();
                    document.querySelectorAll('#nav_BankService_form input, #nav_BankService_form textarea').forEach(el => {
                        el.value = '';
                    });
                    APortalModule._APortalToaster._toastr(1, response.message);
                    $(`${_root} #BankService-Master-Panel`).removeClass('show');
                    APortalBankService._APortalBankService._pageIndex = 0;
                    APortalBankService._APortalBankService.List();
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalBankService.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_LoanProductMaster' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_BankService_form_wrapper`).empty().html(response);
            $(`${_root} #BankService-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
            $(document).ready(function () {
                $('.select2').select2({
                    placeholder: "-- Select --",
                    allowClear: true,
                    width: '100%'
                });
            });
            $(`${_root} #h3_addBankServiceHeader`).empty().html('Edit Service Details');
        });
    };

    _APortalBankService.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_LoanProductMaster', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalBankService._APortalBankService._pageIndex = 0;
                APortalBankService._APortalBankService.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalBankService.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_LoanProductMaster' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalBankService._APortalBankService._pageIndex = 0;
                APortalBankService._APortalBankService.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalBankService.prototype.ExportToExcel = function (SPName, title) {
        let url = module.urls.export;
        let data = {
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: SPName
        };
        APortalModule._APortalAjaxResponse.DownloadFile(url, data, title, function (response) {
            if (response.isSuccess) {
                // Handle successful export
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalBankService.prototype.toggleMenu = function (btn) {
        const menu = btn.nextElementSibling;

        // close other menus first
        document.querySelectorAll(".action-menu").forEach(m => {
            if (m !== menu) m.style.display = "none";
        });

        // toggle current menu
        menu.style.display = (menu.style.display === "block") ? "none" : "block";

        // close on outside click
        const handler = (e) => {
            if (!btn.parentElement.contains(e.target)) {
                menu.style.display = "none";
                document.removeEventListener("click", handler);
            }
        };

        document.addEventListener("click", handler);
    };

    module._APortalBankService = new _APortalBankService();
    module._APortalBankService.init();
})(APortalBankService || window.APortalBankService);