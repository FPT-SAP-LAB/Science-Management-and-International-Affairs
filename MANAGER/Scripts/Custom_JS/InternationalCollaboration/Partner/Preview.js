$('#preview').click(function () {
    var content = $('.summernote').summernote('code');
    var address = $('#partner_address').val()
    var website = $('#partner_website').val()
    var avata = $('#avata').attr('src')
    var temp = {
        content: content,
        website: website,
        address: address,
        avata: avata,
    };
    $.ajax({
        url: '/Partner/Pass_Content',
        type: "POST",
        data: JSON.stringify(temp),
        contentType: "application/json;charset=utf-8",
        cache: false,
        success: function () {
            window.open('/Partner/Preview');
        },
        error: function () {

        }
    });
});