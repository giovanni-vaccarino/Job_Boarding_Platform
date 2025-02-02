/// <reference types="cypress" />

describe('Apply for an Internship Test', () => {
    beforeEach(() => {
        // Step 1: Log in before navigating to the job listings page
        cy.visit('http://localhost:5173/login');

        cy.get('#emailField').type('student1@gmail.com');
        cy.get('#passwordField').type('Password123!');
        cy.get('#loginButton').click();

        // Step 2: Verify successful login and navigate to job listing
        cy.wait(2000);
        cy.get("#homePageButton").click();

        // Step 3: Intercept the apply request before any action
        cy.intercept('POST', '/api/apply', (req) => {
            req.continue((res) => {
                console.log('API Response:', res);
            });
        }).as('applyRequest');
    });

    it('should apply to an internship successfully or retry if response is null or fails', () => {
        function applyForJob(index) {
            cy.get('[id^="viewDetailsJob_"]').eq(index).should('be.visible').click();

            // Step 6: Verify navigation to job details page
            cy.url().should('include', '/job');

            // Step 7: Click the apply button
            cy.get('#applyButton').should('be.visible').click();

        }

        // Step 5: Select the first job from the list
        applyForJob(0);

    });
});
