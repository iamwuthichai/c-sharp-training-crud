﻿namespace TrainingAPI001.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; }
        private int recordsPerPage = 10;
        private readonly int recordsPerPageMax = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set 
            {
                if (value > recordsPerPageMax)
                {
                    recordsPerPage = recordsPerPageMax;
                }
                else {
                    recordsPerPage = value;
                }
            }
        }
    }
}
