var token = localStorage.getItem("token")
if (token != null) {
    var decoded = jwt_decode(token)
    var userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"];
}

$(document).ready(() => {
    loadPopularPost()

    let urlParam = new URLSearchParams(window.location.search);
    let postId = urlParam.get("id");

    loadPostInfo(postId);
    loadPrevNextPost(postId);

    var result;
    
    const makeApiCall = () => {
        let bookBtnDown = $(".bookmark-btn-down")
        let bookBtnUp = $(".bookmark-btn-up")

        return new Promise((resolve) => {
            $.ajax({
                url: "https://localhost:7065/api/Bookmark/CheckBookmarkExist?userId=" + userId + "&postId=" + postId,
                method: "GET",
                contentType: "application/json",
                headers:{
                    Authorization: "Bearer " + token
                },
                success: function (response) {
                    if (response == true) {
                        bookBtnDown.empty()
                        bookBtnDown.append(`<i class="fa-solid fa-bookmark"></i>`)

                        bookBtnUp.empty()
                        bookBtnUp.append(`<i class="fa-solid fa-bookmark"></i>`)

                        resolve(true)
                    } else {
                        resolve(false)
                    }
                },
                error: function (xhr) {
                    SlimNotifierJs.notification(
                        "error",
                        "Error",
                        xhr.responseText,
                        3000
                    );
                },
            });
        });
    };

    (async function () {
        try {
            if (userId != null) {
                result = await makeApiCall();
            } else {
                $(".bookmark-btn-down").click(() => {
                    SlimNotifierJs.notification(
                        "info",
                        "Information",
                        "Need to Login First!",
                        3000
                    );
                })

                $(".bookmark-btn-up").click(() => {
                    SlimNotifierJs.notification(
                        "info",
                        "Information",
                        "Need to Login First!",
                        3000
                    );
                })
            }
        } catch (error) {
            console.error('API error:', error);
        }
    })();

    $(".bookmark-btn-up").click(() => {
        if (result == true) {
            removeBookmark(postId, userId);
            result = false;
        } else {
            addBookmark(postId, userId)
            result = true;
        }
    })

    $(".bookmark-btn-down").click(() => {
        if (result == true) {
            removeBookmark(postId, userId);
            result = false;
        } else {
            addBookmark(postId, userId)
            result = true;
        }
    })
})

function truncate(str, n) {
    return str.length > n ? str.slice(0, n - 1) + "&hellip;" : str;
}

function loadPopularPost() {
    $.ajax({
        url: "https://localhost:7065/api/Post/GetPopularPosts",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            $("#popular-post").empty()
            $("#popular-post").append(
                response.map((post) =>
                    `<a href="../Post/tech-single.html?id=${post.id}" class="list-group-item list-group-item-action flex-column align-items-start">
                        <div class="w-100 justify-content-between">
                            <img src="${post.image}" alt="" class="img-fluid float-left">
                            <h5 class="mb-1">${truncate(post.title, 30)}</h5>
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
}

function loadPostInfo(postId) {
    $.ajax({
        url: "https://localhost:7065/api/Post/GetReadingPostById/" + postId,
        method: "GET",
        contentType: "application/json",
        success: (response) => {
            $(".breadcrumb").append(
                `<li class="breadcrumb-item active">${response.title}</li>`
            )

            $("#post-cate").append(
                `<a href="../Category/tech-category-01.html?cateId=${response.catId}" title="">${response.categoryName}</a>`
            )

            $("#post-title").append(`${response.title}`)

            $("#post-info").append(
                `<small><a href="../post/tech-single.html?id=${response.id}" title="">${response.created}</a></small>
                <small><a href="../author/tech-author.html?creatorId=${response.authorId}" title="">by ${response.authorName}</a></small>
                <small><a href="#" title=""><i class="fa fa-eye"></i> ${response.viewsCount}</a></small>`
            )

            $(".blog-content").append(`${response.content}`)
        },
        error: (xhr) => {
            SlimNotifierJs.notification(
                "error",
                "Error",
                xhr.responseText,
                3000
            );
        }
    })
}

function loadPrevNextPost(postId) {
    $.ajax({
        url: "https://localhost:7065/api/Post/GetPrevAndNextPost/" + postId,
        method: "GET",
        contentType: "application/json",
        success: (response) => {
            let prevPost = response.previousPost
            let nextPost = response.nextPost

            if (prevPost == null) {
                $(".prevnextpost").append(
                    `<div class="row">
                        <div class="col-lg-6">
                            <div class="blog-list-widget">
                                <div class="list-group">
                                    <a href="../post/tech-single.html?id=${nextPost.id}" class="list-group-item list-group-item-action flex-column align-items-start">
                                        <div class="w-100 justify-content-between">
                                            <img src="${nextPost.image}" alt="" class="img-fluid float-left" style="width: 72px; height: 47px;">
                                            <h5 class="mb-1">${truncate(nextPost.title, 30)}</h5>
                                            <small>Next Post</small>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>`
                )
            }

            if (nextPost == null) {
                $(".prevnextpost").append(
                    `<div class="row">
                        <div class="col-lg-6">
                            <div class="blog-list-widget">
                                <div class="list-group">
                                    <a href="../post/tech-single.html?id=${prevPost.id}" class="list-group-item list-group-item-action flex-column align-items-start">
                                        <div class="w-100 justify-content-between text-right">
                                            <img src="${prevPost.image}" alt="" class="img-fluid float-right" style="width: 72px; height: 47px;">
                                            <h5 class="mb-1">${truncate(prevPost.title, 30)}</h5>
                                            <small>Prev Post</small>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>`
                )
            }
            
            if (prevPost != null && nextPost != null) {
                $(".prevnextpost").append(
                    `<div class="row">
                        <div class="col-lg-6">
                            <div class="blog-list-widget">
                                <div class="list-group">
                                    <a href="../post/tech-single.html?id=${prevPost.id}" class="list-group-item list-group-item-action flex-column align-items-start">
                                        <div class="w-100 justify-content-between text-right">
                                            <img src="${prevPost.image}" alt="" class="img-fluid float-right" style="width: 72px; height: 47px;">
                                            <h5 class="mb-1">${truncate(prevPost.title, 30)}</h5>
                                            <small>Prev Post</small>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
    
                        <div class="col-lg-6">
                            <div class="blog-list-widget">
                                <div class="list-group">
                                    <a href="../post/tech-single.html?id=${nextPost.id}" class="list-group-item list-group-item-action flex-column align-items-start">
                                        <div class="w-100 justify-content-between">
                                            <img src="${nextPost.image}" alt="" class="img-fluid float-left" style="width: 72px; height: 47px;">
                                            <h5 class="mb-1">${truncate(nextPost.title, 30)}</h5>
                                            <small>Next Post</small>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>`
                )
            }
        },
        error: (xhr) => {
            SlimNotifierJs.notification(
                "error",
                "Error",
                xhr.responseText,
                3000
            );
        }
    })
}

function addBookmark(postId, userId) {
    let data = {
        postId: postId,
        userId: userId
    }

    let bookBtnDown = $(".bookmark-btn-down")
    let bookBtnUp = $(".bookmark-btn-up")

    $.ajax({
        url: "https://localhost:7065/api/Bookmark/AddBookmark",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        headers:{
            Authorization: "Bearer " + token
        },
        success: function (response) {
            SlimNotifierJs.notification(
                "success",
                "Success",
                "Bookmark added successfully!",
                3000
            );
            bookBtnDown.empty()
            bookBtnDown.append(`<i class="fa-solid fa-bookmark"></i>`)

            bookBtnUp.empty()
            bookBtnUp.append(`<i class="fa-solid fa-bookmark"></i>`)
        },
        error: function (xhr, status, error) {
            SlimNotifierJs.notification(
                "error",
                "Error",
                xhr.responseText,
                3000
            );
        },
    })
}

function removeBookmark(postId, userId) {
    let data = {
        postId: postId,
        userId: userId
    }

    let bookBtnDown = $(".bookmark-btn-down")
    let bookBtnUp = $(".bookmark-btn-up")

    $.ajax({
        url: "https://localhost:7065/api/Bookmark/RemoveBookmark",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        headers:{
            Authorization: "Bearer " + token
        },
        success: function (response) {
            SlimNotifierJs.notification(
                "success",
                "Success",
                "Bookmark removed successfully!",
                3000
            );
            bookBtnDown.empty()
            bookBtnDown.append(`<i class="fa-regular fa-bookmark"></i>`)

            bookBtnUp.empty()
            bookBtnUp.append(`<i class="fa-regular fa-bookmark"></i>`)
        },
        error: function (xhr, status, error) {
            SlimNotifierJs.notification(
                "error",
                "Error",
                xhr.responseText,
                3000
            );
        },
    })
}