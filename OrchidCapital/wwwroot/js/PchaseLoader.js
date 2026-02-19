(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-cportallogin-wrapper';

    // Create root
    let _PChaseLoader = function () { };

    _PChaseLoader.prototype.init = function () {
        module._PChaseLoader.load();
    }

    _PChaseLoader.prototype.load = function () {

    }

    _PChaseLoader.prototype.LoaderStart = function () {
        $(`${_root} .loader`).css('display', 'block');
    }

    _PChaseLoader.prototype.LoaderEnd = function () {
        $(`${_root} .loader`).css('display', 'none');
    }
    module._PChaseLoader = new _PChaseLoader();
    module._PChaseLoader.init();
})(PChaseModule || window.PChaseModule);