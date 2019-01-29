import React from 'react';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';
import SignUpForm from './SignUpForm';

it('renders without crashing', () => {
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