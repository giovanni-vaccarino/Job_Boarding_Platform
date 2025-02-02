/// <reference types="cypress" />

describe('User Registration Test', () => {
  it('should register a new user and redirect to the profile page', () => {
    // Step 1: Visit the registration page
    cy.visit('http://localhost:5173/register'); // Update if necessary

    // Step 2: Fill in the registration form
    cy.get('#emailField').type('student1@gmail.com');
    cy.get('#passwordField').type('Password123!');
    cy.get('#confirmPasswordField').type('Password123!');
    cy.get('#profileField').click(); // Open the dropdown
    cy.get('li[role="option"]').contains('Student').click(); // Select "Student" option from the list
    cy.get('#registerButton').click();

    // Step 3: Verify successful registration
    cy.url().should('include', '/profile');
  });
});
