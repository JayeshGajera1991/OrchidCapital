(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalPages = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'PageName';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalPages.prototype.init = function () {
        module._APortalPages.load();
    }

    _APortalPages.prototype.load = function () {
        APortalPages._APortalPages.List();
    }

    _APortalPages.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_PageMst'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalPages._APortalPages._pageIndex > 0) {
                $(`${_root} #page-tbody-render`).append(response);
            } else if (APortalPages._APortalPages._pageIndex == 0) {
                $(`${_root} #page-tbody-render`).empty();
                $(`${_root} #page-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblPagesList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;;
            var PageSize = PageTotal > $(`${_root} #tblPagesList tbody tr`).length ? $(`${_root} #tblPagesList tbody tr`).length : PageTotal;
            APortalPages._APortalPages._pageIndex = $(`${_root} #tblPagesList tbody tr`).length;
            APortalPages._APortalPages._totalRecords = parseInt($(`${_root} #tblPagesList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalPages.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_PageMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_page_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_page_form').reset();
                document.querySelectorAll('#nav_page_form input, #nav_page_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(document).ready(function () {
                $('.select2').select2({
                    placeholder: "-- Select --",
                    allowClear: true,
                    width: '100%'
                });
            });
            $(`${_root} #h3_addPageHeader`).empty().html('Add New Page Details');
            $(`${_root} #Pages-Master-Panel`).toggleClass('show');
        });
    }

    _APortalPages.prototype.SubmitFormData = function (event, form) {
        $("#nav_page_form").removeData("validator");
        $("#nav_page_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_page_form"));
        event.preventDefault();
        if ($("#nav_page_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_page_form').reset();
                    document.querySelectorAll('#nav_page_form input, #nav_page_form textarea').forEach(el => {
                        el.value = '';
                    });
                    $(`${_root} #Pages-Master-Panel`).removeClass('show');
                    APortalModule._APortalToaster._toastr(1, response.message);
                    APortalPages._APortalPages._pageIndex = 0;
                    APortalPages._APortalPages.List();
                }
                else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalPages.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_PageMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_page_form_wrapper`).empty().html(response);
            $(`${_root} #Pages-Master-Panel`).addClass('show');
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
            $(`${_root} #h3_addPageHeader`).empty().html('Edit Page Details');
        });
    };

    _APortalPages.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_PageMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalPages._APortalPages._pageIndex = 0;
                APortalPages._APortalPages.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalPages.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_PageMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalPages._APortalPages._pageIndex = 0;
                APortalPages._APortalPages.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalPages.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalPages.prototype.toggleMenu = function (btn) {
        // close all open menus first
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

    module._APortalPages = new _APortalPages();
    module._APortalPages.init();
})(APortalPages || window.APortalPages);