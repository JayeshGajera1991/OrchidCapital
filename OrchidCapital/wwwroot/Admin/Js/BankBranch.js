(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalBankBranch = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'BranchName';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalBankBranch.prototype.init = function () {
        module._APortalBankBranch.load();
    }

    _APortalBankBranch.prototype.load = function () {
        APortalBankBranch._APortalBankBranch.List();
    }

    _APortalBankBranch.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_BankBranchMst'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalBankBranch._APortalBankBranch._pageIndex > 0) {
                $(`${_root} #bankBranch-tbody-render`).append(response);
            } else if (APortalBankBranch._APortalBankBranch._pageIndex == 0) {
                $(`${_root} #bankBranch-tbody-render`).empty();
                $(`${_root} #bankBranch-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblBankBranchList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;;
            var PageSize = PageTotal > $(`${_root} #tblBankBranchList tbody tr`).length ? $(`${_root} #tblBankBranchList tbody tr`).length : PageTotal;
            APortalBankBranch._APortalBankBranch._pageIndex = $(`${_root} #tblBankBranchList tbody tr`).length;
            APortalBankBranch._APortalBankBranch._totalRecords = parseInt($(`${_root} #tblBankBranchList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalBankBranch.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_BankBranchMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_BankBranch_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_BankBranch_form').reset();
                document.querySelectorAll('#nav_BankBranch_form input, #nav_BankBranch_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(`${_root} #h3_addBranchHeader`).empty().html('Add New Branch');
            $(`${_root} #BankBranch-Master-Panel`).toggleClass('show');
        });
    }

    _APortalBankBranch.prototype.SubmitFormData = function (event, form) {
        $("#nav_BankBranch_form").removeData("validator");
        $("#nav_BankBranch_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_BankBranch_form"));
        event.preventDefault();
        if ($("#nav_BankBranch_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_BankBranch_form').reset();
                    document.querySelectorAll('#nav_BankBranch_form input, #nav_BankBranch_form textarea').forEach(el => {
                        el.value = '';
                    });
                    APortalModule._APortalToaster._toastr(1, response.message);
                    $(`${_root} #BankBranch-Master-Panel`).removeClass('show');
                    APortalBankBranch._APortalBankBranch._pageIndex = 0;
                    APortalBankBranch._APortalBankBranch.List();
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalBankBranch.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_BankBranchMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_BankBranch_form_wrapper`).empty().html(response);
            $(`${_root} #h3_addBranchHeader`).empty().html('Edit Branch Details');
            $(`${_root} #BankBranch-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
        });
    };

    _APortalBankBranch.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_BankBranchMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalBankBranch._APortalBankBranch._pageIndex = 0;
                APortalBankBranch._APortalBankBranch.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalBankBranch.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_BankBranchMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalBankBranch._APortalBankBranch._pageIndex = 0;
                APortalBankBranch._APortalBankBranch.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalBankBranch.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalBankBranch.prototype.toggleMenu = function (btn) {
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

    module._APortalBankBranch = new _APortalBankBranch();
    module._APortalBankBranch.init();
})(APortalBankBranch || window.APortalBankBranch);