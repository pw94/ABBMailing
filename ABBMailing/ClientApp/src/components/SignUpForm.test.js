import React from 'react';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';
import SignUpForm from './SignUpForm';

describe('SignUpForm', function () {
  beforeEach(() => {
    this.topics = JSON.parse('[{ "id": 1, "name": "Sport" }, { "id": 2, "name": "Health" }]');    
  });

  afterEach(() => {
  });
  
  it('renders without crashing', () => {
    fetch = mockFetch(this.topics);
    
    const div = document.createElement('div');
    ReactDOM.render(
      <MemoryRouter>
        <SignUpForm />
      </MemoryRouter>, div);
  });
  
  it('sends POST when inserted email and selected at least one category', () => {
  });
  
  it('shows alert and does not send POST when inserted incorrect email address', () => {
    
  });
  
  it('shows alert and does not send POST when did not choose any category', () => {
    
  });
  
  it('hides alert when inserted correct email address after the incorrect one', () => {
    
  });
  
  function mockFetch(data) {
    return jest.fn().mockImplementation(() =>
      Promise.resolve({
        ok: true,
        json: () => data
      })
    );
  }
});