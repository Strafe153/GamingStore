import React, { Component, Redirect } from "react";

export class RegisterForm extends Component {
    static displayName = RegisterForm.name;

    constructor(props) {
        super(props);

        this.state = {username: "", password: "", confirmPassword: ""};
        this.onUsernameChange = this.onUsernameChange.bind(this);
        this.onPasswordChange = this.onPasswordChange.bind(this);
        this.onConfirmPasswordChange = this.onConfirmPasswordChange.bind(this);
        this.registerUser = this.registerUser.bind(this);
    }

    onUsernameChange(e) {
        this.setState({username: e.target.value});
    }

    onPasswordChange(e) {
        this.setState({password: e.target.value});
    }

    onConfirmPasswordChange(e) {
        this.setState({confirmPassword: e.target.value});
    }

    async registerUser(e) {
        e.preventDefault();

        fetch("../api/users/register", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                
                return response.text().then(error => { throw new Error(error) });
            })
            .then(() => {
                window.location.href = "/login";
            })
            .catch(error => console.log(error.message));
    }

    render() {
        return <form onSubmit={this.registerUser}>
            <label htmlFor="username">Login:</label>
            <input id="username" type="text" value={this.state.username} onChange={this.onUsernameChange} />
            <br />
            <label htmlFor="password">Password:</label>
            <input id="password" type="password" value={this.state.password} onChange={this.onPasswordChange} />
            <br />
            <label htmlFor="confirm-password">Confirm password:</label>
            <input id="confirm-password" type="password" value={this.state.confirmPassword} onChange={this.onConfirmPasswordChange} />
            <br />
            <input type="submit" value="Sign Up" />
        </form>;
    }
}