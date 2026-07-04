(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalImageConfiguration = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'Headline';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalImageConfiguration.prototype.init = function () {
        module._APortalImageConfiguration.load();
    }

    _APortalImageConfiguration.prototype.load = function () {
        APortalImageConfiguration._APortalImageConfiguration.List();
    }

    _APortalImageConfiguration.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_ImageConfigMst'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalImageConfiguration._APortalImageConfiguration._pageIndex > 0) {
                $(`${_root} #imageconfiguration-tbody-render`).append(response);
            } else if (APortalImageConfiguration._APortalImageConfiguration._pageIndex == 0) {
                $(`${_root} #imageconfiguration-tbody-render`).empty();
                $(`${_root} #imageconfiguration-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblImageConfigurationsList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblImageConfigurationsList tbody tr`).length ? $(`${_root} #tblImageConfigurationsList tbody tr`).length : PageTotal;
            APortalImageConfiguration._APortalImageConfiguration._pageIndex = $(`${_root} #tblImageConfigurationsList tbody tr`).length;
            APortalImageConfiguration._APortalImageConfiguration._totalRecords = parseInt($(`${_root} #tblImageConfigurationsList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalImageConfiguration.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_ImageConfigMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_image_configuration_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_image_config_form').reset();
                document.querySelectorAll('#nav_image_config_form input, #nav_image_config_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(`${_root} #h3_addImageConfigurationHeader`).empty().html('Add New Image Configuration');
            $(`${_root} #ImageConfiguration-Master-Panel`).toggleClass('show');
        });
    }

    _APortalImageConfiguration.prototype.SubmitFormData = function (event, form) {
        $("#nav_image_config_form").removeData("validator");
        $("#nav_image_config_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_image_config_form"));
        event.preventDefault();
        if ($("#nav_image_config_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponsewithFile(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_image_config_form').reset();
                    document.querySelectorAll('#nav_image_config_form input, #nav_image_config_form textarea').forEach(el => {
                        el.value = '';
                    });
                    $(`${_root} #ImageConfiguration-Master-Panel`).removeClass('show');
                    APortalModule._APortalToaster._toastr(1, response.message);
                    APortalImageConfiguration._APortalImageConfiguration._pageIndex = 0;
                    APortalImageConfiguration._APortalImageConfiguration.List();
                }
                else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalImageConfiguration.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_ImageConfigMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_image_configuration_form_wrapper`).empty().html(response);
            $(`${_root} #ImageConfiguration-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
            $(`${_root} #h3_addImageConfigurationHeader`).empty().html('Edit Image Configuration Details');
        });
    };

    _APortalImageConfiguration.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_ImageConfigMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalImageConfiguration._APortalImageConfiguration._pageIndex = 0;
                APortalImageConfiguration._APortalImageConfiguration.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalImageConfiguration.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_ImageConfigMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalImageConfiguration._APortalImageConfiguration._pageIndex = 0;
                APortalImageConfiguration._APortalImageConfiguration.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalImageConfiguration.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalImageConfiguration.prototype.toggleMenu = function (btn) {
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

    module._APortalImageConfiguration = new _APortalImageConfiguration();
    module._APortalImageConfiguration.init();
})(APortalImageConfiguration || window.APortalImageConfiguration);