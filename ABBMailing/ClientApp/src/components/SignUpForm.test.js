import React from 'react';
import { MemoryRouter } from 'react-router-dom';
import { shallow, mount, render } from 'enzyme';
import { expect } from 'chai';
import { Form } from 'reactstrap';
import { SignUpForm } from './SignUpForm';


describe('SignUpForm', function () {
  beforeEach(() => {
    this.topics = JSON.parse('[{ "id": 1, "name": "Sport" }, { "id": 2, "name": "Health" }]');    
  });

  afterEach(() => {
    if (typeof fetch.resetMocks === 'function') {
      fetch.resetMocks()
    }
  });
  
  it('renders without crashing', () => {
    fetch = mockFetch(this.topics);

    render(
      <MemoryRouter>
        <SignUpForm />
      </MemoryRouter>);
  });

  it('fetches topics list during the rendering', () => {
    fetch = mockFetch(this.topics);
    
    mount(
      <MemoryRouter>
        <SignUpForm />
      </MemoryRouter>);
    
    expect(fetch.mock.calls[0][0]).to.be.equal('api/Topics/List');
  });
  
  it('has topics checkboxes and email input', async () => {
    fetch = mockFetch(this.topics);

    const wrapper = shallow(<SignUpForm />);
    await Promise.resolve();

    expect(wrapper.find('[type="checkbox"]')).to.have.lengthOf(this.topics.length);
    expect(wrapper.find('[type="email"]')).to.have.lengthOf(1);
  });

  it('sends POST when inserted email and selected at least one category', async () => {
    fetch = mockFetch(this.topics);
    const wrapper = shallow(<SignUpForm history={[]} />);
    await Promise.resolve();

    wrapper.find('[type="email"]').simulate('change', { target: { value: 'user@server.com' } });
    const checkboxValue = wrapper.find('[type="checkbox"]').get(0).props.value;
    wrapper.find('[type="checkbox"]').first().simulate('change', { target: { checked: true, value: checkboxValue } });
    wrapper.find(Form).simulate('submit', { preventDefault: jest.fn() });

    expect(fetch.mock.calls.length).to.be.equal(2);
    expect(fetch.mock.calls[1][0]).to.be.equal('api/Addresses');
    expect(fetch.mock.calls[1][1].method).to.be.equal('POST');
  });
  
  it('shows alert and does not send POST when inserted incorrect email address', () => {
    
  });
  
  it('shows alert and does not send POST when did not choose any category', () => {
    
  });
  
  it('hides alert when inserted correct email address after the incorrect one', () => {
    
  });
});

function mockFetch(data) {
  return jest.fn().mockImplementation(() =>
    Promise.resolve({
      ok: true,
      json: () => data
    })
  );
}