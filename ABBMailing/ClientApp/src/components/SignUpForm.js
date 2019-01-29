import React, { Component } from 'react';
import { Alert } from 'reactstrap';

export class SignUpForm extends Component {
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
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
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
            alert('A name was submitted: ' + this.state.email + ' | ' + this.state.selectedTopics);
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
        }
        event.preventDefault();
    }

    renderSubscriptionForm(topics) {
        return (
            <form onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <label htmlFor="exampleInputEmail">Email address</label>
                    <input type="email" className="form-control" id="exampleInputEmail" aria-describedby="emailHelp" placeholder="Enter email" value={this.state.value} onChange={this.handleChangeEmail}></input>
                {this.state.emailIncorrect &&
                    <Alert color="danger">
                        Please type in your email
                    </Alert>
                }
                </div>
                <div>
                    <p>Topics</p>
                {topics.map(topic =>
                    <div className="form-check" key={topic.id}>
                        <input className="form-check-input" type="checkbox" value={topic.id} id={'topicCheckbox' + topic.id} onChange={this.handleChangeTopic}></input>
                        <label className="form-check-label" htmlFor={'topicCheckbox' + topic.id}>
                            {topic.name}
                        </label>
                    </div>
                )}
                {this.state.topicsIncorrect &&
                    <Alert color="danger">
                        Please select topics
                    </Alert>
                }
                </div>
                <button type="submit" className="btn btn-primary">Submit</button>
            </form>
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
