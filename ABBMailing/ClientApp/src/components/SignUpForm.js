import React, { Component } from 'react';
import { Alert, Button, Form, FormGroup, Label, Input } from 'reactstrap';

export default class SignUpForm extends Component {
    static displayName = SignUpForm.name;

    constructor(props) {
        super(props);
        this.state = { topics: [], loading: true, selectedTopics: [], email: '', emailIncorrect: false, topicsIncorrect: false};

        this.handleChangeEmail = this.handleChangeEmail.bind(this);
        this.handleChangeTopic = this.handleChangeTopic.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        fetch('api/Topics/List')
            .then(response => response.json())
            .then(data => {
                this.setState({ topics: data, loading: false });
            });
    }

    validateEmail() {
        var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(this.state.email).toLowerCase());
    }

    validateTopics() {
        return this.state.selectedTopics.length > 0;
    }

    handleChangeEmail(event) {
        this.setState({ email: event.target.value });
    }

    handleChangeTopic(event) {
        let selectedTopic = event.target.value;
        let selectedTopics = this.state.selectedTopics;
        if (event.target.checked) {
            selectedTopics.push(selectedTopic);
        } else {
            let index = selectedTopics.indexOf(selectedTopic);
            if (index > -1) {
                selectedTopics.splice(index, 1);
            }
        }
        this.setState({ selectedTopics: selectedTopics });
    }

    handleSubmit(event) {
        let incorrectEmail = !this.validateEmail();
        let incorrectTopics = !this.validateTopics();
        this.setState({emailIncorrect: incorrectEmail, topicsIncorrect: incorrectTopics});
        if (!incorrectEmail && !incorrectTopics) {
            fetch('api/Addresses', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: this.state.email,
                    topics: this.state.selectedTopics,
                })
            })
            .then(() => {
                this.props.history.push('/');
            });
        }
        event.preventDefault();
    }

    renderSubscriptionForm(topics) {
        return (
            <Form onSubmit={this.handleSubmit}>
                <FormGroup>
                    <Label for="exampleInputEmail">Email address</Label>
                    <Input type="email" id="exampleInputEmail" aria-describedby="emailHelp" placeholder="Enter email" value={this.state.value} onChange={this.handleChangeEmail}></Input>
                {this.state.emailIncorrect &&
                    <Alert color="danger">
                        Please type in your correct email
                    </Alert>
                }
                </FormGroup>
                <FormGroup>
                    <p>Topics</p>
                {topics.map(topic =>
                    <FormGroup check key={topic.id}>
                        <Label check>
                        <Input type="checkbox" value={topic.id} id={'topicCheckbox' + topic.id} onChange={this.handleChangeTopic}></Input>
                            {topic.name}
                        </Label>
                    </FormGroup>
                )}
                {this.state.topicsIncorrect &&
                    <Alert color="danger">
                        Please select topics
                    </Alert>
                }
                </FormGroup>
                <Button color="primary">Submit</Button>
            </Form>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderSubscriptionForm(this.state.topics);
        return (
            <div>
                <p>Sign Up Form</p>
                {contents}
            </div>
        );
    }
}
