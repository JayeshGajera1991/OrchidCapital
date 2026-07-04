(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper',
        toastrTimeout = 3000;

    // Create root
    let _APortalMaster = function () { };

    _APortalMaster.prototype.init = function () {
        module._APortalMaster.load();
    }

    _APortalMaster.prototype.load = function () {

    }

    //Pass :- event/this/object(Current root)/DefaultPageSize
    _APortalMaster.prototype.ScrollMore = function (event, $this, obj) {
        if ($($this).scrollTop() + $($this).innerHeight() >= $($this)[0].scrollHeight - 50) {
            if (parseInt(obj._totalRecords) > parseInt(obj._pageIndex)) {
                obj.List();
            }
        }
        event.preventDefault();
    }

    //Pass :- id(Searchbox)/object(Current root)
    _APortalMaster.prototype.SearchBoxReset = function (id, obj) {
        $(`${_root} #` + id).next(".search_close_btn").addClass('hidden');
        $(`${_root} #` + id).val('');
        obj._filter = '';
        obj._pageIndex = 0;
        obj._pageSize = obj.defaultPageSize;
        obj.List();
    }

    //Pass :- event/this/object(Current root)
    _APortalMaster.prototype.InputBoxSearch = function (event, $this, obj) {
        if ($($this).val().length > 0) {
            $($this).next(".search_close_btn").removeClass('hidden');
        } else {
            $($this).next(".search_close_btn").addClass('hidden');
        }
        if (event.keyCode === 13 && $($this).val().length > 0) {
            obj._filter = $($this).val();
            obj._pageIndex = 0;
            obj._pageSize = obj.defaultPageSize;
            obj.List();
        } else if (event.keyCode === 13 && $($this).val().length === 0) {
            obj._filter = '';
            obj._pageIndex = 0;
            obj._pageSize = obj.defaultPageSize;
            obj.List();
        }
    }

    //Pass :- id(Current Table)/name(Column Name)/object(Current root)
    _APortalMaster.prototype.Sorting = function (id, name, obj) {
        let selector = `${_root} #${id}`;

        let isAscActive = $(`${selector} i.${name}_ASC.active`).length > 0;
        let isDescActive = $(`${selector} i.${name}_DESC.active`).length > 0;

        let _sortingType = "ASC";

        if (isAscActive) {
            _sortingType = "DESC";
        } else if (isDescActive) {
            _sortingType = "ASC";
        }

        // Remove all active states
        $(`${selector} i.fa-sort-up, ${selector} i.fa-sort-down`).removeClass('active');

        // Add active to selected
        $(`${selector} i.${name}_${_sortingType}`).addClass('active');

        obj._sortColumn = name;
        obj._sortDirection = _sortingType;
        obj._pageIndex = 0;
        obj._pageSize = obj.defaultPageSize;

        obj.List();
    }

    //Pass :-this/object(Current root)
    _APortalMaster.prototype.StatusChangeEvent = function ($this, obj) {
        obj._active = $($this).val();
        obj._pageIndex = 0;
        obj._pageSize = obj.defaultPageSize;
        obj.List();
    }

    //Pass :-object(Current root)
    _APortalMaster.prototype.FormDataClear = function (obj) {
        obj.Create();
    }

    //Pass :-id(table id)
    _APortalMaster.prototype.FormDataReset = function (id, editId) {
        $(`${_root} #` + id + ' tbody tr #edit-' + editId).click();
    }

    //Pass :-this/id(render div id)
    _APortalMaster.prototype.FormClose = function (id) {
        $(`${_root} #` + id).empty();
        $('body').addClass('fullwidth');
        $('body').removeClass('show_assign_panel');
        $('body ').removeClass('showAssignPanel');
    }
    //Pass :-element for body,class (toggle class)
    _APortalMaster.prototype.ToggleClass = function (element, cls) {
        $(element).toggleClass(cls);
    }
    //Pass :-id (filter div id)
    _APortalMaster.prototype.CloseFilterEvent = function (id) {
        $(`${_root} #` + id).css('display', 'none');
    }
    _APortalMaster.prototype.CloseFilter = function () {
        $(".filter_view_box").addClass("hidden");
        $('.filter_btn').removeClass("active");
    }
    module._APortalMaster = new _APortalMaster();
    module._APortalMaster.init();
})(APortalModule || window.APortalModule);