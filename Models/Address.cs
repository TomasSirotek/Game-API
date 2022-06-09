namespace API.Models; 

public class Address {
    public string Id { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int Zip { get; set; }
    
    public Address(string id, string street,int number,string city,string country, int zip) {
        Id = id;
        Street = street;
        Number = number;
        City = city;
        Country = country;
        Zip = zip;
    }

    public Address() { }
}