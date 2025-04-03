# Notes on Assumptions and Areas for Improvement

Thank you for reviewing my submission. Below is a brief summary of the assumptions I made while working through the exercise, followed by some areas I would approach differently or expand upon with more time.

## Assumptions

- **Intended audience**  
  I assumed this interface was designed for internal use – either for customer service or admin teams – rather than being a public-facing site. The level of detail shown (e.g. pricing, internal metadata) would typically be hidden from end users. In a live system, I’d expect some form of login or user roles to manage this.

- **Front-end performance concerns**  
  I opted to avoid front-end filtering/sorting using JavaScript or libraries, as I wasn’t sure how performant that would be on large datasets. Instead, I chose server-side postbacks for better control and predictability.

- **Single view structure**  
  Everything was kept in a single view, with the table and pagination in a partial view for simplicity and to demonstrate the core functionality clearly. In a production environment, this would be broken down into partial views that could communicate with one another as needed.

- **Filtering implementation**  
  For filtering, I created a helper class in the repository layer to apply pattern matching in memory. I now realise this wasn’t ideal – name filtering didn’t work correctly due to lack of matches, and a stored procedure with SQL regex or `LIKE` would’ve been more suitable.

- **Discount functionality misread**  
  I initially imagined the discount code being part of the payment flow rather than the filter – this was a misread of the user story on my part.

- **Event structure**  
  Integration events are currently stored flat in the EventBus project. In future, I would separate them into `Commands`, `Queries`, and `Responses` to make them easier to manage.

- **VAT and revenue calculations**  
  I estimated the logic around VAT and profit calculations. In a real project, I would speak to the business owner or product manager to clarify exactly what’s required before implementing.

## Improvements I Would Make

Given more time and a live environment, here are the key areas I would improve or approach differently:

### Architecture and Data Flow

- **API-side filtering, sorting, and pagination**  
  These are currently handled in WebMVC after the full dataset is loaded. It would be more efficient to push this logic into the Catalog.API, especially at scale.

- **Only fetch what’s needed**  
  I would avoid passing the full ViewModel back and forth. Instead, I’d split responsibilities and retrieve just the data needed for each action.

- **More graceful error handling**  
  At present, errors are thrown directly from catch blocks for visibility during testing. In production, I would handle these more gracefully and provide helpful user feedback.

- **Better organisation of components**  
  Partial views would be introduced to split the plate list, filters, and statistics into more modular, maintainable components.

- **Cleaner controller structure**  
  I would move the pagination and sorting logic out of the MVC controller and into services or helper classes to keep things focused.

- **External configuration for constants**  
  Values like `pageSize = 20` are currently hardcoded but should ideally come from configuration.
  
- **More XML comments and documentation**
  I would add XML doc comments throughout the codebase to improve maintainability and support future developers in understanding intent and usage.

### Validation and Testing

- **Stronger plate validation**  
  I’d include rules such as a maximum of 8 characters, no symbols, and restrictions on the number of letters and numbers (e.g. no more than 3 numbers, 4 letters). I left this intentionally light as I wasn’t sure of the real-world constraints.

- **More comprehensive unit tests**  
  I would add more targeted tests using mocked data, including tests that simulate exceptions and unusual edge cases to ensure robustness.

- **Clarify calculations with stakeholders**  
  Any future work on revenue and VAT logic would be clarified with product owners or business stakeholders to ensure accuracy.

---

Thanks again for the opportunity to complete this test. I’ve aimed to demonstrate not just a working solution, but also how I reflect on and iterate my work.
