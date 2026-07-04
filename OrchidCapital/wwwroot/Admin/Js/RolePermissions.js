(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalRolePermissions = function () {
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
    _APortalRolePermissions.prototype.init = function () {
        module._APortalRolePermissions.load();
    }

    _APortalRolePermissions.prototype.load = function () {
        var PageTotal = $(`${_root} #rolesTable tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #rolesTable tbody tr`).length ? $(`${_root} #rolesTable tbody tr`).length : PageTotal;
        document.getElementById("pagination").innerText =
            `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
    }

    _APortalRolePermissions.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_UserPageRightsMappingMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #RolePermission-Master-Panel`).empty().html(response);
            $(`${_root} #RolePermission-Master-Panel`).toggleClass('show');
        });
    }

    _APortalRolePermissions.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_UserPageRightsMappingMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #RolePermission-Master-Panel`).empty().html(response);
            $(`${_root} #RolePermission-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
            $("#selEmployee").val($("#hdnUserId").val()).change();
        });
    };

    _APortalRolePermissions.prototype.Delete = function (code) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_UserPageRightsMappingMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
            location.reload();
        });
    };

    _APortalRolePermissions.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalRolePermissions.prototype.Change = function ($this) {
        let url = module.urls.change;
        let data = { Id: $($this).val() };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            let html = "";
            $.each(response, function (index, item) {
                let _IsAdd = ''; let _IsView = '';
                if (item.isAdd) {
                    _IsAdd = 'checked';
                }
                if (item.isView) {
                    _IsView = 'checked';
                }
                html += "<div class='perm-row'>";
                html += "<span id='" + item.pageId + "'>" + item.pageName + "</span>";
                html += "<input type='radio' name='" + item.pageId + "' value='Add' " + _IsAdd + ">";
                html += "<input type='radio' name='" + item.pageId + "' value='View' " + _IsView + ">";
                html += "<div style='color:red; cursor: pointer;' onclick='APortalRolePermissions._APortalRolePermissions.UncheckeRradioBtn(" + item.pageId + ");'><i class='fa-solid fa-trash'></i></div>";
                html += "</div>";
            });
            $("#permissions-tbody-render").empty().html(html);
            $("#EmployeeName").val($("#selEmployee option:selected").data('fname'));
            $("#EmployeeRole").val($("#selEmployee option:selected").data('role'));
            $("#EmployeeId").val('OC000' + $($this).val());
        });
    };

    _APortalRolePermissions.prototype.Next = function () {
        if ($("#selEmployee option:selected").val() === '0') {
            APortalModule._APortalToaster._toastr(0, "Please select user");
            return false;
        }
        const nextBtn = document.querySelector(".next-btn");
        const backBtn = document.getElementById("backBtn");

        const step1 = document.getElementById("step1");
        const step2 = document.getElementById("step2");

        const steps = document.querySelectorAll(".step");
        const process = document.querySelectorAll(".process-box");

        step1.classList.remove("active");
        step2.classList.add("active");

        steps[0].classList.remove("active");
        steps[1].classList.add("active");

        process[0].classList.remove("active");
        process[1].classList.add("active");
    }

    _APortalRolePermissions.prototype.Back = function () {
        const nextBtn = document.querySelector(".next-btn");
        const backBtn = document.getElementById("backBtn");

        const step1 = document.getElementById("step1");
        const step2 = document.getElementById("step2");

        const steps = document.querySelectorAll(".step");
        const process = document.querySelectorAll(".process-box");

        step2.classList.remove("active");
        step1.classList.add("active");

        steps[1].classList.remove("active");
        steps[0].classList.add("active");

        process[1].classList.remove("active");
        process[0].classList.add("active");
    }

    _APortalRolePermissions.prototype.Save = function () {
        let url = module.urls.save;
        let xml = "";
        xml += "<Root>";
        $('#permissions-tbody-render input[type="radio"]:checked').each(function () {
            xml += "<Row>";
            xml += "<UserName>" + $("#selEmployee option:selected").text() + "</UserName>";
            xml += "<UserId>" + $("#selEmployee option:selected").val() + "</UserId>";
            xml += "<RoleId>" + $("#selEmployee option:selected").attr("data-roleid") + "</RoleId>";
            xml += "<PageId>" + $(this).attr('name') + "</PageId>";
            if ($(this).val() === 'Add') {
                xml += "<IsAdd>true</IsAdd>";
                xml += "<IsView>false</IsView>";
            } else if ($(this).val() === 'View') {
                xml += "<IsAdd>false</IsAdd>";
                xml += "<IsView>true</IsView>";
            }
            xml += "</Row>";
        });
        xml += "</Root>";
        let data = { UserId: $("#selEmployee option:selected").val(), XmlData: xml };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalRolePermissions._APortalRolePermissions.Create();
                location.reload();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    }

    _APortalRolePermissions.prototype.toggleMenu = function (btn) {
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

    _APortalRolePermissions.prototype.UncheckeRradioBtn = function (btn) {
        $('input[name="' + btn + '"]:checked').prop('checked', false);
    };

    _APortalRolePermissions.prototype.View = function (code) {
        let url = module.urls.change;
        let data = { Id: code };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            $(`${_root} #permissionModal`).addClass('show');
            let html = "";
            $.each(response, function (index, item) {
                let className = 'block';
                if (item.isAdd) {
                    className = 'full';
                }
                if (item.isView) {
                    className = 'read';
                }
                html += "<div class='perm-item'><span>" + item.pageName + "</span><span class='perm-badge " + className + "'>" + className + "</span></div>";
            });
            $("#modalBody").empty().html(html);
        });
    };

    _APortalRolePermissions.prototype.closeModal = function () {
        $(`${_root} #permissionModal`).removeClass('show');
        $("#modalBody").empty();
    };

    _APortalRolePermissions.prototype.InputBoxSearch = function (event, $this) {
        if ($($this).val().length > 0) {
            $($this).next(".search_close_btn").removeClass('hidden');
        } else {
            $($this).next(".search_close_btn").addClass('hidden');
        }
        if (event.keyCode === 13 && $($this).val().length > 0) {
            var value = $($this).val().toLowerCase();
            $("#rolesTable tbody tr").filter(function () {
                $(this).toggle(
                    $(this).text().toLowerCase().indexOf(value) > -1
                );
            });
        } else if (event.keyCode === 13 && $($this).val().length === 0) {
            var value = $($this).val().toLowerCase();
            $("#rolesTable tbody tr").filter(function () {
                $(this).toggle(
                    $(this).text().toLowerCase().indexOf(value) > -1
                );
            });
        }

    }

    _APortalRolePermissions.prototype.SearchBoxReset = function (id) {
        $(`${_root} #` + id).next(".search_close_btn").addClass('hidden');
        $(`${_root} #` + id).val('');
        var value = $(`${_root} #` + id).val().toLowerCase();
        $("#rolesTable tbody tr").filter(function () {
            $(this).toggle(
                $(this).text().toLowerCase().indexOf(value) > -1
            );
        });
    }

    module._APortalRolePermissions = new _APortalRolePermissions();
    module._APortalRolePermissions.init();
})(APortalRolePermissions || window.APortalRolePermissions);