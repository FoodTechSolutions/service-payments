Feature: PaymentService
  As a user
  I want to process payments
  So that I can pay for invoices

  Scenario: Process payment successfully
    Given an invoice with ID "123" exists and is unpaid
    And a payment method of type "CreditCard"
    When I process the payment for the invoice with ID "123"
    Then the payment should be processed successfully
    And the invoice should be marked as paid

  Scenario: Process payment for an already paid invoice
    Given an invoice with ID "123" exists and is already paid
    And a payment method of type "CreditCard"
    When I process the payment for the invoice with ID "123"
    Then an error should be thrown with the message "Invoice is already paid."

  Scenario: Process payment for a non-existent invoice
    Given an invoice with ID "999" does not exist
    And a payment method of type "CreditCard"
    When I process the payment for the invoice with ID "999"
    Then an error should be thrown with the message "Invoice not found."
