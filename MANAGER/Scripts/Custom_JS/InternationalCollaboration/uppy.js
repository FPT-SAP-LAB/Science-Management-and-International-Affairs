var KTUppy = function () {
    const Tus = Uppy.Tus;
    const StatusBar = Uppy.StatusBar;
    const FileInput = Uppy.FileInput;
    const Informer = Uppy.Informer;

    var initUppy1 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'going_add_officer_upload';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppy1 = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 5242880, // 5mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.pdf', 'image/*', '.jpg', '.jpeg', '.png', '.gif']
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

    var initUppy2 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'going_edit_officer_upload';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppy2 = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 5242880, // 5mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.pdf', 'image/*', '.jpg', '.jpeg', '.png', '.gif']
            }
        });

        uppy2.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppy2.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppy2.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppy2.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppy2.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppy2.on('file-added', function (file) {
            //var sizeLabel = "bytes";
            //var filesize = file.size;
            //if (filesize > 1024) {
            //    filesize = filesize / 1024;
            //    sizeLabel = "kb";

            //    if (filesize > 1024) {
            //        filesize = filesize / 1024;
            //        sizeLabel = "MB";
            //    }
            //}
            var uploadListHtml = uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + '</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            
            $uploadedList.append(uploadListHtml);
            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            var itemId = $(this).attr('data-id');
            uppy2.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    var initUppy3 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'change_status_upload';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppy3 = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 5242880, // 5mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.pdf', 'image/*', '.jpg', '.jpeg', '.png', '.gif']
            }
        });

        uppy3.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppy3.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppy3.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppy3.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppy3.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppy3.on('file-added', function (file) {
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
            uppy3.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    var initUppy4 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'coming_add_officer_upload';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppy4 = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 5242880, // 5mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.pdf', 'image/*', '.jpg', '.jpeg', '.png', '.gif']
            }
        });

        uppy4.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppy4.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppy4.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppy4.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppy4.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppy4.on('file-added', function (file) {
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
            uppy4.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    var initUppy5 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'coming_edit_officer_upload';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppy5 = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 5242880, // 5mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.pdf', 'image/*', '.jpg', '.jpeg', '.png', '.gif']
            }
        });

        uppy5.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppy5.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppy5.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppy5.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppy5.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppy5.on('file-added', function (file) {
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
            uppy5.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    return {
        // public functions
        init: function () {
            initUppy1();
            initUppy2();
            initUppy3();
            initUppy4();
            initUppy5();
        }
    };
}();

KTUtil.ready(function () {
    KTUppy.init();
});