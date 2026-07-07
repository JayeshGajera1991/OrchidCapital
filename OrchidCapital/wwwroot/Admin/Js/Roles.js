(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalRoles = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'Role';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalRoles.prototype.init = function () {
        module._APortalRoles.load();
    }

    _APortalRoles.prototype.load = function () {
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
        APortalRoles._APortalRoles.List();
    }

    _APortalRoles.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_RoleMst'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalRoles._APortalRoles._pageIndex > 0) {
                $(`${_root} #roles-tbody-render`).append(response);
            } else if (APortalRoles._APortalRoles._pageIndex == 0) {
                $(`${_root} #roles-tbody-render`).empty();
                $(`${_root} #roles-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblRolesList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblRolesList tbody tr`).length ? $(`${_root} #tblRolesList tbody tr`).length : PageTotal;
            APortalRoles._APortalRoles._pageIndex = $(`${_root} #tblRolesList tbody tr`).length;
            APortalRoles._APortalRoles._totalRecords = parseInt($(`${_root} #tblRolesList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalRoles.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_RoleMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_roles_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_roles_form').reset();
                document.querySelectorAll('#nav_roles_form input, #nav_roles_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(`${_root} #h3_addRoleHeader`).empty().html('Add New Role Details');
            $(`${_root} #Roles-Master-Panel`).toggleClass('show');
        });
    }

    _APortalRoles.prototype.SubmitFormData = function (event, form) {
        $("#nav_roles_form").removeData("validator");
        $("#nav_roles_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_roles_form"));
        event.preventDefault();
        if ($("#nav_roles_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_roles_form').reset();
                    document.querySelectorAll('#nav_roles_form input, #nav_roles_form textarea').forEach(el => {
                        el.value = '';
                    });
                    APortalModule._APortalToaster._toastr(1, response.message);
                    $(`${_root} #Roles-Master-Panel`).removeClass('show');
                    APortalRoles._APortalRoles._pageIndex = 0;
                    APortalRoles._APortalRoles.List();
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalRoles.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_RoleMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_roles_form_wrapper`).empty().html(response);
            $(`${_root} #Roles-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
            $(`${_root} #h3_addRoleHeader`).empty().html('Edit Role Details');
        });
    };

    _APortalRoles.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_RoleMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalRoles._APortalRoles._pageIndex = 0;
                APortalRoles._APortalRoles.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalRoles.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_RoleMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalRoles._APortalRoles._pageIndex = 0;
                APortalRoles._APortalRoles.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalRoles.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalRoles.prototype.toggleMenu = function (btn) {
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

    module._APortalRoles = new _APortalRoles();
    module._APortalRoles.init();
})(APortalRoles || window.APortalRoles);