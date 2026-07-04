var LoaderCounter = {
    count: 0,
    start: function () {
        if (this.count === 0) {
            APortalModule._APortalLoader.LoaderStart();
        }
        this.count++;
    },
    end: function () {
        this.count--;
        if (this.count <= 0) {
            this.count = 0;
            APortalModule._APortalLoader.LoaderEnd();
        }
    }
};
(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalAjaxResponse = function () { };

    // Page Load After Call This Function
    _APortalAjaxResponse.prototype.init = function () {
        module._APortalAjaxResponse.load();
    }

    _APortalAjaxResponse.prototype.load = function () {

    }

    _APortalAjaxResponse.prototype.GetAjaxHtmlResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "GET",
            contentType: "application/html; charset=utf-8",
            dataType: "html",
            beforeSend: function () {  },
            async: async,
            headers: {
                "Access-Control-Allow-Origin": APortalModule.configs.basePath,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {               
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {               
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _APortalAjaxResponse.prototype.GetAjaxJsonResponse = function (url, data, successCallback, async = true, loader = true) {       
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {  },
            async: async,
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _APortalAjaxResponse.prototype.PostAjaxJsonResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json charset=utf-8",
            dataType: "json",
            async: async,
            beforeSend: function () {  },
            cache: false,
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                
                LoaderCounter.end();
                
            }
        });
    }

    _APortalAjaxResponse.prototype.PostFromAjaxJsonResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            contentType: false,
            processData: false,
            async: async,
            beforeSend: function () {},
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                
                LoaderCounter.end();
               
            }
        });
    }

    _APortalAjaxResponse.prototype.PostValidationAjaxJsonResponse = function ($this, successCallback, async = true, loader = true) {
        var _$this = $($this);
        var frmValues = _$this.serialize();
        if (loader) LoaderCounter.start();
        $.ajax({
            type: _$this.attr('method'),
            url: _$this.attr('action'),
            data: frmValues,
            dataType: "json",
            async: async,
            beforeSend: function () { },
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _APortalAjaxResponse.prototype.PostValidationAjaxJsonResponsewithFile = function ($this, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        var _$this = $($this);
        var _form = $($this)[0];
        var frmValues = new FormData(_form);
        $.ajax({
            type: _$this.attr('method'),
            url: _$this.attr('action'),
            data: frmValues,
            contentType: false, 
            processData: false,
            beforeSend: APortalModule._APortalLoader.LoaderStart(),
            complete: APortalModule._APortalLoader.LoaderEnd(),
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response.responseText);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response.responseText);
            }
        });
    }

    _APortalAjaxResponse.prototype.DownloadFile = function (url, data, _filename, async = true, loader = true)
    {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            beforeSend: function () {  },
            async: async,
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true,
                responseType: 'blob'
            },
            crossDomain: true,
            success: function (response, status, xhr)
            {
                const blob = new Blob([response], { type: xhr.getResponseHeader('Content-Type') });
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = _filename;
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
            },
            error: function (response)
            {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response)
            {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function ()
            {
                LoaderCounter.end();
            }
        });
    }

    _APortalAjaxResponse.prototype.PostFromAjaxProgressbarJsonResponse = function (url, data, mID, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress", function (e) {
                    $("#spndel_" + mID).addClass('hidden');
                    if (e.lengthComputable) {
                        var loaded = e.loaded;
                        var total = e.total;
                        var percent = Math.round((loaded / total) * 100);

                        $("#prg_" + mID).css("width", percent + "%");
                    };
                });
                xhr.addEventListener('load', function () {
                    $("#divprg_" + mID).addClass('hidden');
                    $("#spndel_" + mID).addClass('hidden');
                });
                return xhr;
            },
            url: url,
            data: data,
            type: "POST",
            contentType: false,
            processData: false,
            async: async,
            beforeSend: function () {  },
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _APortalAjaxResponse.prototype.CustomAjaxRequest = function (url, data, type, contentType, dataType, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: type,
            contentType: contentType,
            dataType: dataType,
            async: async,
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

   
    _APortalAjaxResponse.prototype.PostAjaxHtmlResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            contentType: false,
            processData: false,
            async: async,
            beforeSend: function () { },
            cache: false,
            headers: {
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                APortalModule._APortalToaster._error(response);
            },
            failure: function (response) {
                APortalModule._APortalToaster._failure(response);
            },
            complete: function () {

                LoaderCounter.end();

            }
        });
    }

    module._APortalAjaxResponse = new _APortalAjaxResponse();
    module._APortalAjaxResponse.init();
})(APortalModule || window.APortalModule);