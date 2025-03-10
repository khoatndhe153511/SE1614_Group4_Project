﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SE1614_Group4_Project_API.Models
{
    public partial class spriderumContext : DbContext
    {
        public spriderumContext()
        {
        }

        public spriderumContext(DbContextOptions<spriderumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Block> Blocks { get; set; } = null!;
        public virtual DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public virtual DbSet<Cat> Cats { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Datum> Data { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Info> Infos { get; set; } = null!;
        public virtual DbSet<Like> Likes { get; set; } = null!;
        public virtual DbSet<Metum> Meta { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<RelatedCat> RelatedCats { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Unsplash> Unsplashes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<YoutubeDatum> YoutubeData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>(entity =>
            {
                entity.ToTable("block");

                entity.HasIndex(e => e.Id1, "UK_or4i01kr528g9d4o351ixfqnu")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Id1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_id");

                entity.Property(e => e.PostId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("post_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.V).HasColumnName("__v");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Blocks)
                    .HasPrincipalKey(p => p.Id1)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FKaeb1kehuvgl9e7xfkkhoflfrd");
            });

            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PostId })
                    .HasName("bookmark_pk");

                entity.ToTable("bookmark");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bookmark___fk_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bookmark___fk_user");
            });

            modelBuilder.Entity<Cat>(entity =>
            {
                entity.ToTable("cat");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("slug");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.ReplyUserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reply_user_id");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_user");
            });

            modelBuilder.Entity<Datum>(entity =>
            {
                entity.ToTable("data");

                entity.HasIndex(e => e.BlockId, "UK_9dr7ikb6porcbqsv942aatrmm")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Alignment)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("alignment");

                entity.Property(e => e.BlockId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_block_id");

                entity.Property(e => e.Caption)
                    .HasMaxLength(255)
                    .HasColumnName("caption");

                entity.Property(e => e.DockLeft).HasColumnName("dock_left");

                entity.Property(e => e.DockRight).HasColumnName("dock_right");

                entity.Property(e => e.Embed)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("embed");

                entity.Property(e => e.Expanded).HasColumnName("expanded");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Link)
                    .IsUnicode(false)
                    .HasColumnName("link");

                entity.Property(e => e.Service)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("service");

                entity.Property(e => e.Source)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("source");

                entity.Property(e => e.Stretched).HasColumnName("stretched");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.HasOne(d => d.Block)
                    .WithOne(p => p.Datum)
                    .HasPrincipalKey<Block>(p => p.Id1)
                    .HasForeignKey<Datum>(d => d.BlockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1vn6v2fsdujkg87lycf53e2ms");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable("_file");

                entity.HasIndex(e => e.DataId, "UK_gjb9uncu29x91c08nccdcv17e")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DataId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_data_id");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.HasOne(d => d.Data)
                    .WithOne(p => p.File)
                    .HasPrincipalKey<Datum>(p => p.BlockId)
                    .HasForeignKey<File>(d => d.DataId)
                    .HasConstraintName("FKplbe0ulctr50vb30sj8qcuh18");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.MetaId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_meta_id");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.HasOne(d => d.Meta)
                    .WithMany(p => p.Images)
                    .HasPrincipalKey(p => p.DataId)
                    .HasForeignKey(d => d.MetaId)
                    .HasConstraintName("FK8we8hp07bhhxkk32jcmf8985n");
            });

            modelBuilder.Entity<Info>(entity =>
            {
                entity.ToTable("info");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.FileId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_file_id");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Infos)
                    .HasPrincipalKey(p => p.DataId)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FKgrpb8mq63ogby1kabtpkwvl9n");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("like");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsLike).HasColumnName("is_like");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_like_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_like_user");
            });

            modelBuilder.Entity<Metum>(entity =>
            {
                entity.ToTable("meta");

                entity.HasIndex(e => e.DataId, "UK_nj0vv4r2e7sgee6xobwu9ung2")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DataId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_data_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.HasOne(d => d.Data)
                    .WithOne(p => p.Metum)
                    .HasPrincipalKey<Datum>(p => p.BlockId)
                    .HasForeignKey<Metum>(d => d.DataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKajhih277jf6kur78w0a5ahxsr");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.HasIndex(e => e.Id1, "UK_n2gh2ehvh9xiusog1efgw43fg")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.CommentCount).HasColumnName("comment_count");

                entity.Property(e => e.ControlversialPoint).HasColumnName("controlversial_point");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatorId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("creator_id");

                entity.Property(e => e.DatePoint).HasColumnName("date_point");

                entity.Property(e => e.Description)
                    .HasMaxLength(2555)
                    .HasColumnName("description");

                entity.Property(e => e.HotPoint).HasColumnName("hot_point");

                entity.Property(e => e.Id1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_id");

                entity.Property(e => e.IsEditorPick).HasColumnName("is_editor_pick");

                entity.Property(e => e.ModifiedAt).HasColumnName("modified_at");

                entity.Property(e => e.NewTitle)
                    .HasMaxLength(2555)
                    .HasColumnName("new_title");

                entity.Property(e => e.OgImageUrl)
                    .IsUnicode(false)
                    .HasColumnName("og_image_url");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.ReadingTime).HasColumnName("reading_time");

                entity.Property(e => e.Slug)
                    .HasMaxLength(2555)
                    .IsUnicode(false)
                    .HasColumnName("slug");

                entity.Property(e => e.Star).HasColumnName("star");

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("thumbnail");

                entity.Property(e => e.Title)
                    .HasMaxLength(2555)
                    .HasColumnName("title");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.ViewsCount).HasColumnName("views_count");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FKoays9foj0c9ru5sbirxyf8i6t");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FKrcassyc33mut09589ibrv4tma");
            });

            modelBuilder.Entity<RelatedCat>(entity =>
            {
                entity.ToTable("related_cat");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.PostId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("post_id");

                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("slug");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.RelatedCats)
                    .HasPrincipalKey(p => p.Id1)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FKnqx0hnps3x3u52mw5vw1jxyav");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PostId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("post_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Tags)
                    .HasPrincipalKey(p => p.Id1)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK7tk5hi5tl1txftyn44dtq2mv6");
            });

            modelBuilder.Entity<Unsplash>(entity =>
            {
                entity.ToTable("unsplash");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Author)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("author");

                entity.Property(e => e.DataId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_data_id");

                entity.Property(e => e.ProfileLink)
                    .IsUnicode(false)
                    .HasColumnName("profile_link");

                entity.HasOne(d => d.Data)
                    .WithMany(p => p.Unsplashes)
                    .HasPrincipalKey(p => p.BlockId)
                    .HasForeignKey(d => d.DataId)
                    .HasConstraintName("FKexohpoplpg2t54sn8rdrfsfav");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.Birth)
                    .HasColumnType("date")
                    .HasColumnName("birth");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(255)
                    .HasColumnName("display_name");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Gravatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("gravatar");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Role).HasColumnName("role");
            });

            modelBuilder.Entity<YoutubeDatum>(entity =>
            {
                entity.ToTable("youtube_data");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ChannelAvatarUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("channel_avatar_url");

                entity.Property(e => e.ChannelTitle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("channel_title");

                entity.Property(e => e.ChannelUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("channel_url");

                entity.Property(e => e.PostId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("_post_id");

                entity.Property(e => e.Star).HasColumnName("star");

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("thumbnail");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.Property(e => e.ViewCount).HasColumnName("view_count");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.YoutubeData)
                    .HasPrincipalKey(p => p.Id1)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FKmnah7999fydanoeal5nb9761c");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
