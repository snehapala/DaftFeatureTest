Feature: Daft.KeywordFilter.Test
	Keyword filter functionality Assesment

@mytag
Scenario: Verify Keyword filter on the daft website
	Given I have navigated to the Daft website
	And search for a saleAd in dublin county
	And there are search results for the county
	When I apply keyword filter '<filter>'
	And there are results for that filter
	Then open one search result and verify garage keyword is there on that advert

Examples:
| filter |
| garage |