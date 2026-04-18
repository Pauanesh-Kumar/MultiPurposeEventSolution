using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Request
{
    public record class ModifyPoemDto
    {
        public int Id { get; set; }
        public string? Title { get; init; }
        public string? Content { get; init; }

        public string? Category { get; init; }

        public string? Tags { get; init; }
        public string? Dedication { get; init; }

        public string? Mood { get; init; }

        public PoemStatus Status { get; init; }

        public DateTimeOffset? CreatedAt { get; init; }

        public string? AuthorId { get; init; }
    }
}
