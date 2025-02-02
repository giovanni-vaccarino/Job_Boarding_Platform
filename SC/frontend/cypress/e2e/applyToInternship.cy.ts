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
        cy.get("#homePageButton").click()
    });



    it('should apply to an internship successfully', () => {

        // Step 3: Select a job from the list
        cy.get('[id^="viewDetailsJob_"]').first().click();

        // Step 4: Verify navigation to job details page
        cy.url().should('include', '/job');

        // Step 5: Click the apply button
        cy.get('#applyButton').click();

        // Step 6: Verify success message or redirection
        cy.url().should('include', '/confirm');
        cy.get('[data-test="confirmation-message"]').should('contain', 'Application Sent Successfully');
    });
});
