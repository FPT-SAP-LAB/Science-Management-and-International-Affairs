var KTUppy = function () {
    const Tus = Uppy.Tus;
    const StatusBar = Uppy.StatusBar;
    const FileInput = Uppy.FileInput;
    const Informer = Uppy.Informer;

    var initUppy1 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'add_banmem_DongToChuc';
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
                allowedFileTypes: ['.pdf', '.jfif', '.jpg', '.jpeg', '.png']
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
            if (uppy1.getFiles().length != 0) {
                //trigger remove file
                $(id + ' .uppy-list .uppy-list-remove').click();
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
        let elemId = 'edit_banmem_DongToChuc';
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
                allowedFileTypes: ['.pdf', '.jfif', '.jpg', '.jpeg', '.png']
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
            if (uppy2.getFiles().length != 0) {
                //trigger remove file
                $(id + ' .uppy-list .uppy-list-remove').click();
            }
            file_action = 'edit';
            var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);

            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            file_action = 'remove';
            var itemId = $(this).attr('data-id');
            uppy2.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }
    var initUppyDuTruAdd = function () {
        let elemId = 'kt_uppy_addKPDuTru';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppyDuTruAdd = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 3145728, // 3mb
                allowedFileTypes: ['image/*', '.xlsx', '.xls', '.csv', '.pdf']
            }
        });

        uppyDuTruAdd.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppyDuTruAdd.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppyDuTruAdd.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppyDuTruAdd.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppyDuTruAdd.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppyDuTruAdd.on('file-added', function (file) {
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
            if (uppyDuTruAdd.getFiles().length != 0) {
                //trigger remove file
                $(id + ' .uppy-list .uppy-list-remove').click();
            }
            var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);
            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            var itemId = $(this).attr('data-id');
            uppyDuTruAdd.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    var initUppyDuTruEdit = function () {
        let elemId = 'kt_uppy_editKPDuTru';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppyDuTruEdit = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 3145728, // 3mb
                allowedFileTypes: ['image/*', '.xlsx', '.xls', '.csv', '.pdf']
            }
        });

        uppyDuTruEdit.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppyDuTruEdit.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppyDuTruEdit.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppyDuTruEdit.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });
        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppyDuTruEdit.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppyDuTruEdit.on('file-added', function (file) {
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
            if (uppyDuTruEdit.getFiles().length != 0) {
                //trigger remove file
                $(id + ' .uppy-list .uppy-list-remove').click();
            }
            file_action = 'edit';
            var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);
            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            file_action = 'remove';
            var itemId = $(this).attr('data-id');
            uppyDuTruEdit.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    var initUppyDieuChinhEdit = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'kt_uppy_editKPDieuChinh';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppyDieuChinhEdit = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 3145728, // 3mb
                allowedFileTypes: ['image/*', '.xlsx', '.xls', '.csv', '.pdf']
            }
        });

        uppyDieuChinhEdit.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppyDieuChinhEdit.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppyDieuChinhEdit.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppyDieuChinhEdit.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppyDieuChinhEdit.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppyDieuChinhEdit.on('file-added', function (file) {
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
            if (uppyDieuChinhEdit.getFiles().length != 0) {
                //trigger remove file
                $(id + ' .uppy-list .uppy-list-remove').click();
            }
            file_action = 'edit';
            var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);
            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            file_action = 'remove';
            var itemId = $(this).attr('data-id');
            uppyDieuChinhEdit.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }

    var initUppyThucTeEdit = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        let elemId = 'kt_uppy_editKPThucTe';
        let id = '#' + elemId;
        let $statusBar = $(id + ' .uppy-status');
        let $uploadedList = $(id + ' .uppy-list');
        let timeout;

        uppyThucTeEdit = Uppy.Core({
            debug: true,
            autoProceed: false,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 3145728, // 3mb
                allowedFileTypes: ['image/*', '.xlsx', '.xls', '.csv', '.pdf']
            }
        });

        uppyThucTeEdit.use(FileInput, {
            target: id + ' .uppy-wrapper',
            pretty: false
        });
        uppyThucTeEdit.use(Informer, {
            target: id + ' .uppy-informer'
        });

        // demo file upload server
        uppyThucTeEdit.use(Tus, {
            endpoint: 'https://master.tus.io/files/'
        });
        uppyThucTeEdit.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppyThucTeEdit.on('upload', function () {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppyThucTeEdit.on('file-added', function (file) {
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
            if (uppyThucTeEdit.getFiles().length != 0) {
                //trigger remove file
                $(id + ' .uppy-list .uppy-list-remove').click();
            }
            file_action = 'edit';
            var uploadListHtml = '<div class="uppy-list-item" data-id="' + file.id + '"><div class="uppy-list-label">' + file.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + file.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);
            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            file_action = 'remove';
            var itemId = $(this).attr('data-id');
            uppyThucTeEdit.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }
    return {
        init: function () {
            initUppy1();
            initUppy2();
            initUppyDuTruAdd();
            initUppyDuTruEdit();
            initUppyDieuChinhEdit();
            initUppyThucTeEdit();
        }
    };
}();

KTUtil.ready(function () {
    KTUppy.init();
});