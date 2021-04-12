var QsDecreaseID = 0;

$("#qs_university").select2({
    placeholder: "Tên đại học top QS",
    allowClear: true,
    width: '100%',
    ajax: {
        url: "/QsUniversity/List",
        dataType: 'json',
        delay: 250,
        data: function (params) {
            return {
                university: params.term, // search term
                page: params.page
            };
        },
        processResults: function (data, params) {
            data.map(function (obj) {
                obj.id = obj.university;
                return obj;
            });
            if (data.length == 0) {
                data.push({
                    id: params.term,
                    university: params.term,
                    ranking: "Không xác định"
                });
                QsDecreaseID = QsDecreaseID - 1;
            }

            return {
                results: data
            };
        },
        cache: true
    },
    escapeMarkup: function (markup) {
        return markup;
    }, // let our custom formatter work
    minimumInputLength: 2,
    templateResult: formatQs, // omitted for brevity, see the source of this page
    templateSelection: formatQsSelection // omitted for brevity, see the source of this page
});

function formatQs(repo) {
    if (repo.loading) return repo.text;
    var markup = "<div class='clearfix'><div>" + repo.university + "</div><div>Ranking: " + repo.ranking + "</div></div>";
    return markup;
}

function formatQsSelection(repo) {
    return repo.university || repo.text;
}