using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniProject____API.Data;
using MiniProject____API.Models;
using MiniProject____API.Models.Dtos;
using MiniProject____API.Models.ViewModels;
using MiniProject____API.Services;
using System;
using System.Net;

namespace MiniProject____API.Services
{
    public class ApiHandler
    {
        public static IResult GetInterests(ApplicationContext context)
        {
            // Retrieve a list of interests with their IDs and titles
            var interests = context.Interests
                    .Select(p => new InterestViewModel()
                    {
                        InterestId = p.Id,
                        Title = p.Title,
                    })
                    .ToList();

            // Return the interests as a JSON result
            return Results.Json(interests);
        }

        public static IResult AddInterest(ApplicationContext context, int interestId, InterestDto interestDto)
        {
            
            // Create an Interest object with the provided information
            Interest interest = new Interest()
            {
                Id = interestId,
                Title = interestDto.Title,
                Description = interestDto.Description,
            };

            // Add the interest to the context and save changes
            context.Interests.Add(interest);
            context.SaveChanges();

            // Return a success status code (201 - Created)
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult GetPersonInterests(ApplicationContext context, int personId)
        {
            // Retrieve a person with their interests based on the provided person ID             
            Person person = context.People
                            .Include(p => p.Interests)
                            .Single(p => p.Id == personId);

            
            if (person == null)
            {
                return Results.NotFound($"Person with ID {personId} not found.");
            }

            // Retrieve and filter the person's interests
            List<PersonInterestViewModel> personInterests =
                person.Interests
                .Select(i => new PersonInterestViewModel()
                {
                    Title = i.Title,
                    Description = i.Description,
                })
                .ToList();

            // Return the person's interests as a JSON result
            return Results.Json(personInterests);
        }

        public static IResult AddInterestLink(ApplicationContext context, int personId, int interestId, InterestLinksDto interestLinks)
        {
            // Retrieve the person and interest based on the provided IDs
            Person person = context.People
                .Single(p => p.Id == personId);

            Interest interest = context.Interests
                .Single(i => i.Id == interestId);

            // Check if the person is found
            if (person == null)
            {
                return Results.NotFound($"Person with ID {personId} not found.");
            }
            //Check if the interest is found
            if (interest == null)
            {
                return Results.NotFound($"Interest with ID {interestId} not found.");
            }

            // Add a new InterestLink to the context with the provided URL, person, and interest
            context.InterestsLinks
                .Add(new InterestLink()
                {
                    Url = interestLinks.Url,
                    Person = person,
                    Interest = interest
                });

            // Save changes to the context
            context.SaveChanges();
            // Return a success status code (201 - Created)
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult GetPeople(ApplicationContext context)
        {
            // Retrieve a list of people with their IDs and names
            List<PersonViewModel> people = context.People
            .Select(p => new PersonViewModel()
            {
                PersonId = p.Id,
                Name = $"{p.FirstName} {p.LastName}"
            })
            .ToList();

            // Return the list of people as a JSON result
            return Results.Json(people);
        }

        public static IResult AddPerson(ApplicationContext context, PersonDto personDto)
        {
            try
            {

                // Create a Person object with the provided information
                Person person = new Person()
                {
                    
                    FirstName = personDto.FirstName,
                    LastName = personDto.LastName,
                    PhoneNumber = personDto.PhoneNumber,

                };

                // Add the person to the context and save changes
                context.People.Add(person);
                context.SaveChanges();

                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                
                return Results.Text(ex.Message);
            }
        }

        public static IResult ConnectPersonAndInterest(ApplicationContext context, int personId, int interestId )
        {
            try
            {
                // Retrive the interest
                Interest interest = context.Interests
                    .Single(i => i.Id == interestId);

                if (interest == null)
                {
                    return Results.NotFound($"Interest with ID {interestId} not found.");
                }

                // Retrive person and their interests
                var person = context.People
                    .Include(p => p.Interests)
                    .FirstOrDefault(p => p.Id == personId);

                //check if person is found
                if (person == null)
                {
                    return Results.NotFound($"Person with ID {personId} not found.");
                }

                person.Interests.Add(interest);
                context.SaveChanges();

                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {              
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }


        }

        public static IResult GetLinksConnectedToPerson(ApplicationContext context, int personId)
        {
            try
            {
                // Retrieve the person and their interests based on the provided person ID
                var person = context.People
                    .Include(p => p.Interests)
                    .FirstOrDefault(p => p.Id == personId);

                if (person == null)
                {
                    return Results.NotFound($"Person with ID {personId} not found.");
                }
                // Retrieve interest links connected to the person's interests
                List<InterestLinkViewModel> interestLinks =
                    context.InterestsLinks
                    .Where(il => person.Interests.Any(i => i.Id == il.InterestId))
                    .Select(il => new InterestLinkViewModel
                    {
                        Url = il.Url
                    })
                    .ToList();

                return Results.Json(interestLinks);
            }
            catch (Exception ex)
            {
                // Return an error result with status code 500 (Internal Server Error)
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
