var KTUppy = function () {
    const Tus = Uppy.Tus;
    const StatusBar = Uppy.StatusBar;
    const FileInput = Uppy.FileInput;
    const Informer = Uppy.Informer;

    var initUppy1 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        var elemId = 'kt_uppy_1';
        var id = '#' + elemId;
        var $statusBar = $(id + ' .uppy-status');
        var $uploadedList = $(id + ' .uppy-list');
        var timeout;

        uppy1 = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 15728640, // 15mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.pdf', '.docx', '.doc']
            }
        });

        uppy1.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppy1.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppy1.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppy1.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppy1.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppy1.on('file-added', function (file) {
            var sizeLabel = "bytes";
            var filesize = file.size;
            if (filesize > 1024) {
                filesize = filesize / 1024;
                sizeLabel = "kb";

                if (filesize > 1024) {
                    filesize = filesize / 1024;
                    sizeLabel = "MB";
                }
            }
            var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);

            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            var itemId = $(this).attr('data-id');
            uppy1.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }
    //var initUppy2 = function () {
    //    // Uppy variables
    //    // For more info refer: https://uppy.io/
    //    var elemId = 'kt_uppy_2';
    //    var id = '#' + elemId;
    //    var $statusBar = $(id + ' .uppy-status');
    //    var $uploadedList = $(id + ' .uppy-list');
    //    var timeout;

    //    uppy2 = Uppy.Core({
    //        debug: true,
    //        autoProceed: false,
    //        showProgressDetails: true,
    //        restrictions: {
    //            maxFileSize: 15728640, // 15mb
    //            maxNumberOfFiles: 1,
    //            minNumberOfFiles: 1,
    //            allowedFileTypes: ['.pdf']
    //        }
    //    });

    //    uppy2.use(FileInput, {
    //        target: id + ' .uppy-wrapper',
    //        pretty: false
    //    });
    //    uppy2.use(Informer, {
    //        target: id + ' .uppy-informer'
    //    });

    //    // demo file upload server
    //    uppy2.use(Tus, {
    //        endpoint: 'https://master.tus.io/files/'
    //    });
    //    uppy2.use(StatusBar, {
    //        target: id + ' .uppy-status',
    //        hideUploadButton: true,
    //        hideAfterFinish: false
    //    });

    //    $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
    //    $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

    //    var $fileLabel = $(id + ' .uppy-input-label');

    //    uppy2.on('upload', function () {
    //        $fileLabel.text("Uploading...");
    //        $statusBar.addClass('uppy-status-ongoing');
    //        $statusBar.removeClass('uppy-status-hidden');
    //        clearTimeout(timeout);
    //    });

    //    uppy2.on('file-added', function (file) {
    //        var sizeLabel = "bytes";
    //        var filesize = file.size;
    //        if (filesize > 1024) {
    //            filesize = filesize / 1024;
    //            sizeLabel = "kb";

    //            if (filesize > 1024) {
    //                filesize = filesize / 1024;
    //                sizeLabel = "MB";
    //            }
    //        }
    //        var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
    //        $uploadedList.append(uploadListHtml);

    //        $statusBar.addClass('uppy-status-hidden');
    //        $statusBar.removeClass('uppy-status-ongoing');
    //    });

    //    $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
    //        var itemId = $(this).attr('data-id');
    //        uppy2.removeFile(itemId);
    //        $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
    //    });
    //}

    return {
        // public functions
        init: function () {
            initUppy1();
            //initUppy2();
            // initUppy6();
        }
    };
}();

KTUtil.ready(function () {
    KTUppy.init();
});