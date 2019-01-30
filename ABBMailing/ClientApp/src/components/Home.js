import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <p>Click and subscribe to our service</p>
        <a className="btn btn-info" href="/sign-up" role="button">Sign Up</a>
      </div>
    );
  }
}
