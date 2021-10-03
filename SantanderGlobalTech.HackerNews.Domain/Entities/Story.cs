using System;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Domain.Entities
{
    public class Story
    {
        /// <summary>
        /// The title of the story
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The URL of the story
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The username of the story's author
        /// </summary>
        public string By { get; set; }

        /// <summary>
        /// Creation date of the story
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// The story's score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Collection of HackerNews IDs the belongs to this Story a.k.a. comments
        /// </summary>
        public IEnumerable<int> Kids { get; set; }
    }
}
