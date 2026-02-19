(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-cportallogin-wrapper',
        toastrTimeout = 3000;

    // Create root
    let _PChanseMaster = function () { };

    _PChanseMaster.prototype.init = function () {
        module._PChanseMaster.load();
    }

    _PChanseMaster.prototype.load = function () {

    }

    //Pass :- event/this/object(Current root)/DefaultPageSize
    _PChanseMaster.prototype.ScrollMore = function (event, $this, obj) {
        if ($($this).scrollTop() + $($this).innerHeight() + 5 >= $($this)[0].scrollHeight) {
            if (parseInt(obj._TotalRecords) > parseInt(obj._PageIndex)) {
                obj.List();
            }
        }
        event.preventDefault();
    }

    //Pass :- id(Searchbox)/object(Current root)
    _PChanseMaster.prototype.SearchBoxReset = function (id, obj) {
        $(`${_root} #` + id).next(".search_close_btn").addClass('hidden');
        $(`${_root} #` + id).val('');
        obj._Filter = '';
        obj._PageIndex = 0;
        obj._PageSize = obj._DefaultPageSize;
        obj.List();
    }

    //Pass :- event/this/object(Current root)
    _PChanseMaster.prototype.InputBoxSearch = function (event, $this, obj) {
        if ($($this).val().length > 0) {
            $($this).next(".search_close_btn").removeClass('hidden');
        } else {
            $($this).next(".search_close_btn").addClass('hidden');
        }
        if (event.keyCode === 13 && $($this).val().length > 0) {
            obj._Filter = $($this).val();
            obj._PageIndex = 0;
            obj._PageSize = obj._DefaultPageSize;
            obj.List();
        } else if (event.keyCode === 13 && $($this).val().length === 0) {
            obj._Filter = '';
            obj._PageIndex = 0;
            obj._PageSize = obj._DefaultPageSize;
            obj.List();
        }
    }

    //Pass :- id(Current Table)/name(Column Name)/object(Current root)
    _PChanseMaster.prototype.Sorting = function (id, name, obj) {
        let _sortingType = "ASC";
        // Set DESC
        $(`${_root} #` + id + ' .bottom_triangle.' + name + '_' + _sortingType + '.active').each(function () {
            _sortingType = "DESC";
        });

        // Deleted active
        $(`${_root} #` + id + ' .bottom_triangle').each(function () {
            $(this).removeClass('active');
        });

        // Deleted active
        $(`${_root} #` + id + ' .top_triangle').each(function () {
            $(this).removeClass('active');
        });

        // Add active
        $(`${_root} #` + id + ' span.' + name + "_" + _sortingType).addClass('active');

        obj._Sorting = name + ' ' + _sortingType;
        obj._PageIndex = 0;
        obj._PageSize = obj._DefaultPageSize;
        obj.List();
    }

    //Pass :-this/object(Current root)
    _PChanseMaster.prototype.StatusChangeEvent = function ($this, obj) {
        obj._IsActive = $($this).val();
        obj._PageIndex = 0;
        obj._PageSize = obj._DefaultPageSize;
        obj.List();
    }

    //Pass :-object(Current root)
    _PChanseMaster.prototype.FormDataClear = function (obj) {
        obj.Create();
    }

    //Pass :-id(table id)
    _PChanseMaster.prototype.FormDataReset = function (id) {
        $(`${_root} #` + id + ' tbody tr.active a:first').click();
    }

    //Pass :-this/id(render div id)
    _PChanseMaster.prototype.FormClose = function (id) {
        $(`${_root} #` + id).empty();
        $('body').addClass('fullwidth');
        $('body').removeClass('show_assign_panel');
        $('body ').removeClass('showAssignPanel');       
    }
    //Pass :-element for body,class (toggle class)
    _PChanseMaster.prototype.ToggleClass = function (element,cls) {
        $(element).toggleClass(cls);
    }
    //Pass :-id (filter div id)
    _PChanseMaster.prototype.CloseFilterEvent = function (id) {
        $(`${_root} #` + id).css('display', 'none');
    }
    _PChanseMaster.prototype.CloseFilter = function () {
        $(".filter_view_box").addClass("hidden");
        $('.filter_btn').removeClass("active");
    }
    module._PChanseMaster = new _PChanseMaster();
    module._PChanseMaster.init();
})(PChaseModule || window.PChaseModule);