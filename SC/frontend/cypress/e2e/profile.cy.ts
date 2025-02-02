/// <reference types="cypress" />

describe('User Profile Page Test', () => {
  beforeEach(() => {
    // Step 1: Log in before visiting the profile page
    cy.visit('http://localhost:5173/login');

    cy.get('#emailField').type('student1@gmail.com');
    cy.get('#passwordField').type('Password123!');
    cy.get('#loginButton').click();

    // Step 2: Verify successful login
    cy.url().should('include', '/profile');
  });

  it('should display user profile details', () => {
    // Step 3: Verify profile information is visible
    cy.get('h1').should('contain', 'Profile'); // Adjust this selector based on actual structure
    cy.get('#emailField').should('have.value', 'student1@gmail.com'); // Check email field
  });

  it('should update the user name successfully', () => {
    // Step 4: Edit and update the user name
    cy.get('[data-test="edit-name"]').click(); // Replace with actual selector
    cy.get('[data-test="name-input"]').clear().type('Updated Name');
    cy.get('[data-test="save-name"]').click(); // Replace with actual selector

    // Step 5: Verify update success
    cy.get('[data-test="name-display"]').should('contain', 'Updated Name');
  });

  it('should allow the user to log out', () => {
    // Step 6: Click the logout button
    cy.get('[data-test="logout-button"]').click(); // Replace with actual selector

    // Step 7: Verify redirection to login page
    cy.url().should('include', '/login');
  });
});
