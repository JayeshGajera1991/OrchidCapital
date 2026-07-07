(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _AFinanceLoanApprovalList = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 0;
        this._sortColumn = 'ApplicationNo';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _AFinanceLoanApprovalList.prototype.init = function () {
        module._AFinanceLoanApprovalList.load();
    }

    _AFinanceLoanApprovalList.prototype.load = function () {
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
        AFinanceLoanApprovalList._AFinanceLoanApprovalList.List();
    }

    _AFinanceLoanApprovalList.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_LoanApprovalList'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (AFinanceLoanApprovalList._AFinanceLoanApprovalList._pageIndex > 0) {
                $(`${_root} #approvallist-tbody-render`).append(response);
            } else if (AFinanceLoanApprovalList._AFinanceLoanApprovalList._pageIndex == 0) {
                $(`${_root} #approvallist-tbody-render`).empty();
                $(`${_root} #approvallist-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblLoanapprovallist tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblLoanapprovallist tbody tr`).length ? $(`${_root} #tblLoanapprovallist tbody tr`).length : PageTotal;
            AFinanceLoanApprovalList._AFinanceLoanApprovalList._pageIndex = $(`${_root} #tblLoanapprovallist tbody tr`).length;
            AFinanceLoanApprovalList._AFinanceLoanApprovalList._totalRecords = parseInt($(`${_root} #tblLoanapprovallist tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _AFinanceLoanApprovalList.prototype.Edit = function (code, step) {
        let _code = APortalModule._APortalCommon.EncryptParameter(code);
        let _step = APortalModule._APortalCommon.EncryptParameter(step);
        window.location.href = "/Finance/LoanApproval?LoanApplicationId=" + _code;
    };

    _AFinanceLoanApprovalList.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_LoanApprovalList', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                AFinanceLoanApprovalList._AFinanceLoanApprovalList._pageIndex = 0;
                AFinanceLoanApprovalList._AFinanceLoanApprovalList.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _AFinanceLoanApprovalList.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_LoanApprovalList' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                AFinanceLoanApprovalList._AFinanceLoanApprovalList._pageIndex = 0;
                AFinanceLoanApprovalList._AFinanceLoanApprovalList.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _AFinanceLoanApprovalList.prototype.ExportToExcel = function (SPName, title) {
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

    _AFinanceLoanApprovalList.prototype.toggleMenu = function (btn) {
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

    module._AFinanceLoanApprovalList = new _AFinanceLoanApprovalList();
    module._AFinanceLoanApprovalList.init();
})(AFinanceLoanApprovalList || window.AFinanceLoanApprovalList);