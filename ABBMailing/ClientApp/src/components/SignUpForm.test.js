import React from 'react';
import { MemoryRouter } from 'react-router-dom';
import { shallow, mount, render } from 'enzyme';
import { expect } from 'chai';
import { Alert, Form } from 'reactstrap';
import { SignUpForm } from './SignUpForm';


describe('SignUpForm', function () {
  beforeEach(() => {
    this.topics = JSON.parse('[{ "id": 1, "name": "Sport" }, { "id": 2, "name": "Health" }]');
    fetch = mockFetch(this.topics); // eslint-disable-line no-native-reassign
  });

  it('renders without crashing', () => {
    render(
      <MemoryRouter>
        <SignUpForm />
      </MemoryRouter>);
  });

  it('fetches topics list during the rendering', () => {
    mount(
      <MemoryRouter>
        <SignUpForm />
      </MemoryRouter>);

    expect(fetch.mock.calls[0][0]).to.be.equal('api/Topics/List');
  });

  it('has topics checkboxes and email input', async () => {
    const component = shallow(<SignUpForm />);
    await Promise.resolve();

    expect(component.find('[type="checkbox"]')).to.have.lengthOf(this.topics.length);
    expect(component.find('[type="email"]')).to.have.lengthOf(1);
  });

  it('sends POST when inserted email, selected one category and then submitted', async () => {
    const component = shallow(<SignUpForm history={[]} />);
    await Promise.resolve();

    const userEmail = 'user@server.com';
    component.find('[type="email"]').simulate('change', { target: { value: userEmail } });
    const topicId = component.find('[type="checkbox"]').get(0).props.value;
    component.find('[type="checkbox"]').first().simulate('change', { target: { checked: true, value: topicId } });
    component.find(Form).simulate('submit', { preventDefault: jest.fn() });

    expect(fetch.mock.calls.length).to.be.equal(2);
    expect(fetch.mock.calls[1][0]).to.be.equal('api/Addresses');
    expect(fetch.mock.calls[1][1].method).to.be.equal('POST');
    expect(JSON.parse(fetch.mock.calls[1][1].body)).to.be.eql({ topics: [topicId], email: userEmail });
  });

  it('sends POST when inserted email, selected two categories and then submitted', async () => {
    const component = shallow(<SignUpForm history={[]} />);
    await Promise.resolve();

    const userEmail = 'user@server.com';
    component.find('[type="email"]').simulate('change', { target: { value: userEmail } });
    const firstTopicId = component.find('[type="checkbox"]').get(0).props.value;
    component.find('[type="checkbox"]').first().simulate('change', { target: { checked: true, value: firstTopicId } });
    const secondTopicId = component.find('[type="checkbox"]').get(1).props.value;
    component.find('[type="checkbox"]').last().simulate('change', { target: { checked: true, value: secondTopicId } });
    component.find(Form).simulate('submit', { preventDefault: jest.fn() });

    expect(fetch.mock.calls.length).to.be.equal(2);
    expect(fetch.mock.calls[1][0]).to.be.equal('api/Addresses');
    expect(fetch.mock.calls[1][1].method).to.be.equal('POST');
    expect(JSON.parse(fetch.mock.calls[1][1].body)).to.be.eql({ topics: [firstTopicId, secondTopicId], email: userEmail });
  });

  it('shows alert and does not send POST when did not change values and submitted', async () => {
    const component = shallow(<SignUpForm />);
    await Promise.resolve();

    component.find(Form).simulate('submit', { preventDefault: jest.fn() });

    expect(fetch.mock.calls.length).to.be.equal(1);
    expect(component.find(Alert).length).to.be.equal(2);
  });

  it('shows alert and does not send POST when inserted incorrect email address and then submitted', async () => {
    const component = shallow(<SignUpForm />);
    await Promise.resolve();

    const userEmail = 'incorrect.email';
    component.find('[type="email"]').simulate('change', { target: { value: userEmail } });
    const topicId = component.find('[type="checkbox"]').get(0).props.value;
    component.find('[type="checkbox"]').first().simulate('change', { target: { checked: true, value: topicId } });
    component.find(Form).simulate('submit', { preventDefault: jest.fn() });

    expect(fetch.mock.calls.length).to.be.equal(1);
    expect(component.find(Alert).length).to.be.equal(1);
  });

  it('shows alert and does not send POST when did not choose any category and submitted', async () => {
    const component = shallow(<SignUpForm />);
    await Promise.resolve();

    const userEmail = 'user@server.com';
    component.find('[type="email"]').simulate('change', { target: { value: userEmail } });
    component.find(Form).simulate('submit', { preventDefault: jest.fn() });

    expect(fetch.mock.calls.length).to.be.equal(1);
    expect(component.find(Alert).length).to.be.equal(1);
  });

  it('shows alert and does not send POST when unselected all categories and then submitted', async () => {
    const component = shallow(<SignUpForm />);
    await Promise.resolve();

    const userEmail = 'user@server.com';
    component.find('[type="email"]').simulate('change', { target: { value: userEmail } });
    component.find(Form).simulate('submit', { preventDefault: jest.fn() });
    const topicId = component.find('[type="checkbox"]').get(0).props.value;
    component.find('[type="checkbox"]').first().simulate('change', { target: { checked: true, value: topicId } });
    component.find('[type="checkbox"]').first().simulate('change', { target: { checked: false, value: topicId } });

    expect(fetch.mock.calls.length).to.be.equal(1);
    expect(component.find(Alert).length).to.be.equal(1);
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