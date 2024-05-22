namespace SuperAdmin.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        public string ServiceName { get; set; }


        public string? Description { get; set; }


        public string? Quantity { get; set; }

        public Double Price { get; set; }
        public float? Priceperunit { get; set; }



    }

}
