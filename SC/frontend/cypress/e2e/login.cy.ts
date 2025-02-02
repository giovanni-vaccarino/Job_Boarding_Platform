describe('User Login Test', () => {
  it('should login successfully and see the dashboard', () => {
    // Step 1: Visit the React frontend
    cy.visit('http://localhost:5173/login'); // Update with your frontend URL

    // Step 2: Fill in the login form
    cy.get('#emailField').type('student1@gmail.com');
    cy.get('#passwordField').type('Password123!');
    cy.get('#loginButton').click();

    // Step 3: Verify successful login
    cy.url().should('include', '/profile');
  });
});
