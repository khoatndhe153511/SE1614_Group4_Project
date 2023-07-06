function truncate(str, n) {
    return str.length > n ? str.slice(0, n - 1) + "&hellip;" : str;
}

$(document).ready(() => {
    $(document).ajaxStart(() => {
        $("#loading-data").show();
    });

    $(document).ajaxStop(() => {
        $("#loading-data").hide();
    });

    loadRecentPost();

    $.ajax({
        url: "https://localhost:7065/api/Post/GetPopularPosts",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            $("#popular-post").empty()
            $("#popular-post").append(
                response.map((post) =>
                    `<a href="./Post/tech-single.html" class="list-group-item list-group-item-action flex-column align-items-start">
                        <div class="w-100 justify-content-between">
                            <img src="${post.image}" alt="" class="img-fluid float-left">
                            <h5 class="mb-1">5 ${truncate(post.description, 30)}</h5>
                            <small>${post.createdAt}</small>
                        </div>
                    </a>`
                )
            );
        },
        error: function (xhr, status, error) {
            SlimNotifierJs.notification(
                "error",
                "Error",
                xhr.responseText,
                3000
            );
        },
    });
})

var page = 1;
var pageSize = 10;

function loadRecentPost() {
    $.ajax({
        url: "https://localhost:7065/api/Post/GetRecentPosts?page=" + page + "&pageSize=" + pageSize,
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            var data = response.posts;
            var totalPages = response.totalPages;
            var recentNews = $("#recent-news");
            recentNews.empty();
            recentNews.append(
                data.map((post) =>
                    `<div class="blog-box row">
                        <div class="col-md-4">
                            <div class="post-media">
                                <a href="./Post/tech-single.html" title="">
                                    <img src="${post.image}" alt="${post.title}" class="img-fluid" style="width: 255px; height: 213px">
                                    <div class="hovereffect"></div>
                                </a>
                            </div>
                        </div>

                        <div class="blog-meta big-meta col-md-8">
                            <h4><a href="./Post/tech-single.html" title="">${post.title}</p>
                            <small class="firstsmall">
                                <a class="bg-orange" href="./Category/tech-category-01.html" title="">${post.categoryName}</a>
                            </small>
                            <small><a href="./Post/tech-single.html?id=${post.id}" title="">21 July, 2017</a></small>
                            <small><a href="./author/tech-author.html?creatorId=${post.authorId}" title="">${post.authorName}</a></small>
                            <small>
                                <a href="./Post/tech-single.html?id=${post.id}" title="">
                                    <i class="fa fa-eye"></i> ${post.viewsCount}
                                </a>
                            </small>
                        </div>
                    </div>

                    <hr class="invis">`
                )
            );

            var pagination = $("#pagination");
            pagination.empty();

            var startPage = Math.max(1, page - 2);
            var endPage = Math.min(startPage + 4, totalPages);

            if (startPage > 1) {
                pagination.append(
                    `<li class="page-item"><a class="page-link">...</a></li>`
                )
            }

            for (var i = startPage; i <= endPage; i++) {
                pagination.append(
                    `<li class="page-item"><a class="page-link">${i}</a></li>`
                )
            }

            if (endPage < totalPages) {
                pagination.append(
                    `<li class="page-item"><a class="page-link">...</a></li>`
                )
            }

            pagination.on("click", "a", function () {
                var clickedPage = parseInt($(this).text());

                if (isNaN(clickedPage)) {
                    if ($(this).text() === "...") {
                        if ($(this).parent().index() === 0) {
                            page = Math.max(1, page - 5);
                        } else {
                            page = Math.min(page + 5, totalPages);
                        }
                    }
                } else {
                    page = clickedPage;
                }

                loadRecentPost();
            });
        },
        error: function (xhr, status, error) {
            SlimNotifierJs.notification(
                "error",
                "Error",
                xhr.responseText,
                3000
            );
        },
    });
}