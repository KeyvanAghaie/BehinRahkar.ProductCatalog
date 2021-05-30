namespace Behin.Product.Test
{
    public class MockData
    {
        public static DAL.Entities.Product[] MockProductSamples()
        {
            return new DAL.Entities.Product[]
            {
                new DAL.Entities.Product()
                {
                    Id = 1001,
                    Code = "100",
                    Name = "Recorder1",
                    Deleted = false,
                    Photo = null ,
                    Price = 10500
                }, 
                new DAL.Entities.Product()
                {
                    Id = 1002,
                    Code = "101",
                    Name = "Recorder2",
                    Deleted = false,
                    Photo = null ,
                    Price = 10501
                },   
                new DAL.Entities.Product()
                {
                    Id = 1003,
                    Code = "102",
                    Name = "Recorder3",
                    Deleted = false,
                    Photo = null ,
                    Price = 10502
                }, 
                new DAL.Entities.Product()
                {
                    Id = 1004,
                    Code = "103",
                    Name = "Recorder4",
                    Deleted = false,
                    Photo = null ,
                    Price = 10503
                },
       
            };
        }

    }
}
