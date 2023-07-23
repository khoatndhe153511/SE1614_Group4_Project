using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils.Interfaces;
using System.Text;

namespace SE1614_Group4_Project_API.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        readonly spriderumContext _;
        private readonly ILogicHandler _logicHandler;

        public PostRepository(spriderumContext spriderumContext, ILogicHandler logicHandler) : base(spriderumContext)
        {
            _ = spriderumContext;
            _logicHandler = logicHandler;
        }

        public new Task Add(Post entity)
        {
            _.Posts.Add(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public int CountTotalCommentByUserId(string userId)
        {
            int count = 0;
            if (userId == null) throw new ArgumentNullException("userId");
            var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
            foreach (var post in posts)
            {
                count += (int)post.CommentCount;
            }

            return count;
            throw new NotImplementedException();
        }

        public int CountTotalPostByUserId(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            return _.Posts.Count(x => x.CreatorId.Equals(userId));
            throw new NotImplementedException();
        }

        public int CountTotalViewByUserId(string userId)
        {
            int count = 0;
            if (userId == null) throw new ArgumentNullException("userId");
            var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
            foreach (var post in posts)
            {
                count += (int)post.ViewsCount;
            }

            return count;
            throw new NotImplementedException();
        }

        public new Task Delete(Post entity)
        {
            _.Posts.Remove(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(params object?[]? key)
        {
            var foundRecord = Find(key);
            _.Posts.Remove(foundRecord);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Post Find(params object?[]? objects)
        {
            var findResult = _.Posts.Find(objects);
            return findResult ?? throw new NullReferenceException("Record not found");
            throw new NotImplementedException();
        }

        public new Task<List<Post>> GetAll()
        {
            var Results = _.Posts.ToListAsync();
            return Results;
            throw new NotImplementedException();
        }

        public List<PostResponseDTO> GetAllPostByUserId(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            var posts = _.Posts
                .Include(_ => _.Cat)
                .Include(_ => _.Creator)
                .Where(x => x.CreatorId.Equals(userId)).OrderByDescending(x => x.CreatedAt).Select(
                _ => new PostResponseDTO
                {
                    id = _.Id,
                    Title = _.Title,
                    isEditorPick = _.IsEditorPick,
                    AuthorName = _.Creator.Name,
                    CategoryName = _.Cat.Name,
                    Created = $"{_.CreatedAt:dd MMM, yyyy}",
                    Modified = $"{_.ModifiedAt:dd MMM, yyyy}"
                })
                .ToList();
            return posts;
            throw new NotImplementedException();
        }

        public new DbSet<Post> GetDbSet()
        {
            return _.Posts;
            throw new NotImplementedException();
        }

        public int TotalPointByUserId(string userId)
        {
            int count = 0;
            if (userId == null) throw new ArgumentNullException("userId");
            var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
            foreach (var post in posts)
            {
                count += (int)post.Point;
            }

            return count;
            throw new NotImplementedException();
        }

        public new Task Update(Post entity)
        {
            _.Posts.Update(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public void UpdatePostRecently(UpdatePostDTO entity)
        {
            List<string> contents = new List<string>();
            string[] tags = { "<p>", "<img>", "<a>", "<h2>", "<h3>", "<blockquote>", "<iframe>" };

            string[] paragraphs = entity.Content.Split(
                    new[] { "<p>", "</p>", "</img>", "</h3>", "</h2>", "</blockquote>", "</a>", "</iframe>" },
                    StringSplitOptions.RemoveEmptyEntries);



            var blocks = _.Blocks.Include(_ => _.Datum).OrderByDescending(p => p.Id).Where(_ => _.PostId == entity.PostId).ToList();
            var lastBlockId = _.Blocks.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();
            var lastDataId = _.Data.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();
            var blockCount = blocks.Count();
            while (paragraphs.Length > blockCount)
            {
                var newBlock = new Block()
                {
                    Id = lastBlockId + 1,
                    Id1 = Guid.NewGuid().ToString(),
                    PostId = entity.PostId,
                    CreatedAt = DateTime.Now,
                    Status = 1,
                    UpdatedAt = DateTime.Now
                };
                var newData = new Datum() { Id = lastDataId + 1, BlockId = newBlock.Id1 };
                _.Blocks.Add(newBlock);
                _.Data.Add(newData);
                lastDataId++;
                lastBlockId++;
                _.SaveChanges();
                blockCount++;
            }

            var nullBlocks = blocks.Where(p => p.Type == null).ToList();
            foreach (var block in nullBlocks)
            {
                _.Database.ExecuteSqlRaw($"DELETE FROM data WHERE _block_id = '{block.Id1}'");
            }
            _.Blocks.RemoveRange(nullBlocks);
            _.SaveChanges();

            while (paragraphs.Length < blockCount)
            {
                var lastBlock = blocks.FirstOrDefault();
                if (lastBlock != null)
                {
                    var lastData = _.Data.FirstOrDefault(d => d.BlockId == lastBlock.Id1);
                    if (lastData != null)
                    {
                        _.Data.Remove(lastData);
                        _.SaveChanges();
                    }
                    blocks.Remove(lastBlock);
                    _.Blocks.Remove(lastBlock);

                    _.SaveChanges();
                    blockCount--;
                }
            }


            for (int i = 0; i < paragraphs.Length; i++)
            {
                var newBlocks = _.Blocks.Where(_ => _.PostId == entity.PostId).ToList();
                string firstTag = _logicHandler.GetFirstTag(paragraphs[i]);
                Block block = newBlocks.ElementAt(i);
                Datum datum = _.Data.Where(_ => _.BlockId == block.Id1).First();
                switch (firstTag)
                {
                    case "h2":
                        block.Type = "biggerHeader";
                        datum.Text = paragraphs[i].Split("<h2>")[1].ToString();
                        break;
                    case "h3":
                        block.Type = "smallerHeader";
                        datum.Text = paragraphs[i].Split("<h3>")[1].ToString();
                        break;
                    case "img":
                        block.Type = "image";
                        datum.Url = _logicHandler.GetNode(paragraphs[i], "img", "src");
                        break;
                    case "iframe":
                        block.Type = "embed";
                        datum.Embed = _logicHandler.GetNode(paragraphs[i], "iframe", "src");
                        datum.Width = int.Parse(_logicHandler.GetNode(paragraphs[i], "iframe", "width"));
                        datum.Height = int.Parse(_logicHandler.GetNode(paragraphs[i], "iframe", "height"));
                        break;
                    case "a":
                        block.Type = "linkTool";
                        datum.Link = _logicHandler.GetNode(paragraphs[i], "a", "href");
                        break;
                    case "blockquote":
                        block.Type = "quote";
                        datum.Text = paragraphs[i].Split("<blockquote>")[1].ToString();
                        break;
                    default:
                        block.Type = "paragraph";
                        datum.Text = paragraphs[i].ToString();
                        break;
                }

                _.Blocks.Update(block);
                _.Data.Update(datum);
                _.SaveChanges();
            }

            var post = _.Posts.Find(entity.Id);
            var author = _.Users.Where(_ => _.Name.Equals(entity.Author)).Select(_ => _.Id).First();

            if (post != null)
            {
                post.Title = entity.Title;
                post.Description = entity.Description;
                post.CreatorId = author;
                post.CatId = entity.CategoryId;
                post.Slug = entity.Slug;
                post.OgImageUrl = entity.ogImageUrl;
                _.Posts.Update(post);
                _.SaveChanges();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public List<PostResponseDTO> GetPostsRecently()
        {
            return _.Posts
                .Include(_ => _.Creator)
                .Include(_ => _.Cat)
                .Where(_ => _.IsEditorPick != false)
                .OrderByDescending(_ => _.CreatedAt)
                .Select(_ => new PostResponseDTO
                {
                    id = _.Id,
                    Image = _.OgImageUrl,
                    Description = _.Description,
                    Created = $"{_.CreatedAt:dd MMM, yyyy}",
                    Modified = $"{_.ModifiedAt:dd MMM, yyyy}",
                    isEditorPick = _.IsEditorPick,
                    Title = _.Title,
                    CategoryId = _.CatId,
                    CategoryName = _.Cat.Name,
                    AuthorId = _.CreatorId,
                    AuthorName = _.Creator.Name,
                    ViewsCount = _.ViewsCount
                }).ToList();
        }

        public void AddPostRecently(AddPostDTO entity)
        {
            string TempId = "";
            try
            {
                var author = _.Users.Where(_ => _.Name.Equals(entity.Author)).FirstOrDefault();
                var category = _.Cats.Where(_ => _.Id == entity.CategoryId).First();
                var postId = _.Posts.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();

                if (author != null)
                {
                    var post = new Post
                    {
                        Id = postId + 1,
                        Id1 = Guid.NewGuid().ToString(),
                        Creator = author,
                        Cat = category,
                        CatId = category.Id,
                        Slug = entity.Slug,
                        ViewsCount = 0,
                        CommentCount = 0,
                        Point = 0,
                        OgImageUrl = entity.ogImageUrl,
                        Description = entity.Description,
                        Title = entity.Title,
                        CreatedAt = DateTime.Now,
                    };

                    TempId = post.Id1;
                    _.Posts.Add(post);
                    _.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException();
                }

                List<string> contents = new List<string>();
                string[] tags = { "<p>", "<img>", "<a>", "<h2>", "<h3>", "<blockquote>", "<iframe>" };
                string[] paragraphs =
                    entity.Content.Split(
                        new[] { "<p>", "</p>", "</img>", "</h3>", "</h2>", "</blockquote>", "</a>", "</iframe>" },
                        StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < paragraphs.Length; i++)
                {
                    string firstTag = _logicHandler.GetFirstTag(paragraphs[i]);
                    var lastBlockId = _.Blocks.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();

                    for (int j = 0; j < paragraphs.Length; j++)
                    {
                        _.Blocks.Add(new Block()
                        {
                            Id = lastBlockId + 1,
                            Id1 = Guid.NewGuid().ToString(),
                            PostId = TempId,
                            CreatedAt = DateTime.Now,
                            Status = 1,
                            UpdatedAt = DateTime.Now
                        });
                        lastBlockId++;
                    }

                    _.SaveChanges();

                    var lastDataId = _.Data.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();
                    var blocks = _.Blocks.Where(_ => _.PostId == TempId).ToList();
                    foreach (var item in blocks)
                    {
                        _.Data.Add(new Datum() { Id = lastDataId + 1, BlockId = item.Id1 });
                        lastDataId++;
                    }

                    _.SaveChanges();
                    Block block = blocks.ElementAt(i);
                    Datum datum = _.Data.Where(_ => _.BlockId == block.Id1).First();

                    switch (firstTag)
                    {
                        case "h2":
                            block.Type = "biggerHeader";
                            datum.Text = paragraphs[i].Split("<h2>")[1].ToString();
                            break;
                        case "h3":
                            block.Type = "smallerHeader";
                            datum.Text = paragraphs[i].Split("<h3>")[1].ToString();
                            break;
                        case "img":
                            block.Type = "image";
                            datum.Url = _logicHandler.GetNode(paragraphs[i], "img", "src");
                            break;
                        case "iframe":
                            block.Type = "embed";
                            datum.Embed = _logicHandler.GetNode(paragraphs[i], "iframe", "src");
                            datum.Width = int.Parse(_logicHandler.GetNode(paragraphs[i], "iframe", "width"));
                            datum.Height = int.Parse(_logicHandler.GetNode(paragraphs[i], "iframe", "height"));
                            break;
                        case "a":
                            block.Type = "linkTool";
                            datum.Link = _logicHandler.GetNode(paragraphs[i], "a", "href");
                            break;
                        case "blockquote":
                            block.Type = "quote";
                            datum.Text = paragraphs[i].Split("<blockquote>")[1].ToString();
                            break;
                        default:
                            block.Type = "paragraph";
                            datum.Text = paragraphs[i].ToString();
                            break;
                    }

                    _.Blocks.Update(block);
                    _.Data.Update(datum);
                    _.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void UpdateStatus(UpdateStatusDTO entity)
        {
            var post = _.Posts.Find(entity.Id);
            post.IsEditorPick = entity.Status;

            _.Posts.Update(post);
            _.SaveChangesAsync();
        }

        public string GetTextPost(int id)
        {
            string result = "";
            try
            {
                var post = _.Posts.Find(id);
                var blocks = _.Blocks.Include(_ => _.Datum).Where(_ => _.PostId == post.Id1).ToList();
                foreach (var item in blocks)
                {
                    if (item.Datum != null)
                    {
                        switch (item.Type)
                        {
                            case "biggerHeader":
                                result = result + "<h2>" + item.Datum.Text + "</h2>";
                                break;
                            case "smallerHeader":
                                result = result + "<h3>" + item.Datum.Text + "</h3>";
                                break;
                            case "image":
                                result = result + "<figure class=\"image\"><img src=\"" + item.Datum.Url + "\" alt=\"" +
                                         item.Datum.Caption + "\"> </img></figure>";
                                break;
                            case "embed":
                                result = result + "<iframe src=\"" + item.Datum.Embed + "\" width=\"" +
                                         item.Datum.Width + "\" height=\"" + item.Datum.Height + "\"></iframe>\r\n";
                                break;
                            case "linkTool":
                                result = result + "<a href=\"" + item.Datum.Link + "\">" + item.Datum.Link + "</a>";
                                break;
                            case "unsplash":
                                result = result + "<figure class=\"image\"><img src=\"" + item.Datum.Url +
                                         "\" alt=\"PostImage\"> </img></figure>";
                                break;
                            case "quote":
                                result = result + "<blockquote>" + item.Datum.Text + "</blockquote>";
                                break;
                            case "pullquote":
                                result = result + "<blockquote>" + item.Datum.Text + "</blockquote>";
                                break;
                            default:
                                result = result + "<p>" + item.Datum.Text + "</p>";
                                break;
                        }
                    }
                    else
                    {
                        result = result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public List<Post> GetPopularPosts()
        {
            return _.Posts.OrderByDescending(x => x.ViewsCount).Take(3).ToList();
            throw new NotImplementedException();
        }

        public List<Post> SearchPosts(string title)
        {
            return _.Posts.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();
        }
        public RateResponseDTO GetRate(int id)
        {
            var like = _.Likes.Include(_ => _.Post).Where(_ => _.PostId == id && _.IsLike == true).Count();
            var dislike = _.Likes.Include(_ => _.Post).Where(_ => _.PostId == id && _.IsLike == false).Count();

            RateResponseDTO rate = new RateResponseDTO();

            rate.likeCount = like;
            rate.disLikeCount = dislike;
            return rate;
        }
        public bool? GetRatesbyUserId(int postId, string userId)
        {
            var rate = _.Likes
                .Include(_ => _.User)
                .Include(_ => _.Post)
                .Where(_ => _.UserId.Equals(userId) && _.PostId == postId)
                .FirstOrDefault();
            if(rate != null)
            {
                return rate.IsLike;
            }
            else
            {
                return null;
            }
        }
        public void UpdateRate(int postId, string userId, bool? like)
        {
            var rate = _.Likes
                .Include(_ => _.User)
                .Include(_ => _.Post)
                .Where(_ => _.UserId.Equals(userId) && _.PostId == postId)
                .FirstOrDefault();
            if(rate  != null)
            {
                rate.IsLike = like;
                _.Likes.Update(rate);
                _.SaveChanges();
            }
            else
            {
                Like newRate = new Like();
                newRate.PostId = postId;
                newRate.UserId = userId;
                newRate.IsLike = like;
                _.Likes.Add(newRate);
                _.SaveChanges();
            }
        }
    }
}