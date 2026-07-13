(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalLoanType = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'Name';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalLoanType.prototype.init = function () {
        module._APortalLoanType.load();
    }

    _APortalLoanType.prototype.load = function () {
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
        APortalLoanType._APortalLoanType.List();
    }

    _APortalLoanType.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_LoanProductTypeMst'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalLoanType._APortalLoanType._pageIndex > 0) {
                $(`${_root} #loantype-tbody-render`).append(response);
            } else if (APortalLoanType._APortalLoanType._pageIndex == 0) {
                $(`${_root} #loantype-tbody-render`).empty();
                $(`${_root} #loantype-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblLoanTypeList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblLoanTypeList tbody tr`).length ? $(`${_root} #tblLoanTypeList tbody tr`).length : PageTotal;
            APortalLoanType._APortalLoanType._pageIndex = $(`${_root} #tblLoanTypeList tbody tr`).length;
            APortalLoanType._APortalLoanType._totalRecords = parseInt($(`${_root} #tblLoanTypeList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalLoanType.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_LoanProductTypeMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_loantype_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_loantype_form').reset();
                document.querySelectorAll('#nav_loantype_form input, #nav_loantype_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(`${_root} #h3_addRoleHeader`).empty().html('Add New Loan Type Details');
            $(`${_root} #LoanTYpe-Master-Panel`).toggleClass('show');
        });
    }

    _APortalLoanType.prototype.SubmitFormData = function (event, form) {
        $("#nav_loantype_form").removeData("validator");
        $("#nav_loantype_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_loantype_form"));
        event.preventDefault();
        if ($("#nav_loantype_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_loantype_form').reset();
                    document.querySelectorAll('#nav_loantype_form input, #nav_loantype_form textarea').forEach(el => {
                        el.value = '';
                    });
                    APortalModule._APortalToaster._toastr(1, response.message);
                    $(`${_root} #LoanTYpe-Master-Panel`).removeClass('show');
                    APortalLoanType._APortalLoanType._pageIndex = 0;
                    APortalLoanType._APortalLoanType.List();
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalLoanType.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_LoanProductTypeMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_loantype_form_wrapper`).empty().html(response);
            $(`${_root} #LoanTYpe-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
            $(`${_root} #h3_addRoleHeader`).empty().html('Edit Loan Type Details');
        });
    };

    _APortalLoanType.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_LoanProductTypeMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalLoanType._APortalLoanType._pageIndex = 0;
                APortalLoanType._APortalLoanType.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalLoanType.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_LoanProductTypeMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalLoanType._APortalLoanType._pageIndex = 0;
                APortalLoanType._APortalLoanType.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalLoanType.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalLoanType.prototype.toggleMenu = function (btn) {
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

    module._APortalLoanType = new _APortalLoanType();
    module._APortalLoanType.init();
})(APortalLoanType || window.APortalLoanType);