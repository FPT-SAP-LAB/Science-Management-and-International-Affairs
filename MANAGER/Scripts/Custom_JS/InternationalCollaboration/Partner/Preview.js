$('#preview').click(function () {
    var content = $('.summernote').summernote('code');
    var address = $('#partner_address').val()
    var website = $('#partner_website').val()
    var imgInp = $('#imgInp').val();
    var temp = { content: content, website: website, address: address, imgInp: imgInp };
    $.ajax({
        url: './pass_content',
        type: "POST",
        data: JSON.stringify(temp),
        contentType: "application/json;charset=utf-8",
        cache: false,
        success: function (data) {
            window.open('./Preview');
        },
        error: function () {

        }
    });
});