using System;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Data.Common.Test.Domain.Article
{
    public class ArticleEntity : EntityBase, ISoftDelete
    {
        public int CategoryId { get; set; }

        [Length(300)]
        public string Title { get; set; }

        [Length(0)]
        public string Content { get; set; }

        [Column("IsPublished")]
        public bool Published { get; set; }

        public DateTime? PublishedTime { get; set; }

        public bool Deleted { get; set; }

        public Guid? DeletedBy { get; set; }

        public string Deleter { get; set; }

        public DateTime? DeletedTime { get; set; }
    }
}
