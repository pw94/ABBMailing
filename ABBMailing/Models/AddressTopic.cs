namespace ABBMailing.Models
{
    public class AddressTopic
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}