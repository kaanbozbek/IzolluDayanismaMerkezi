window.bootstrapSelect = window.bootstrapSelect || {};

(function () {
    function ensurePlugin() {
        return window.jQuery && window.jQuery.fn && window.jQuery.fn.selectpicker;
    }

    window.bootstrapSelect.init = function () {
        if (!ensurePlugin()) {
            console.warn("bootstrap-select plug-in is not available.");
            return;
        }

        window.setTimeout(function () {
            window.jQuery(".selectpicker").selectpicker();
        }, 0);
    };

    window.bootstrapSelect.refresh = function () {
        if (!ensurePlugin()) {
            return;
        }

        window.setTimeout(function () {
            window.jQuery(".selectpicker").selectpicker("refresh");
        }, 0);
    };
})();

window.downloadFile = function (filename, base64Data) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + base64Data;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
