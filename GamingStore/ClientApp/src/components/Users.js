import React, { Component } from "react";

export class Users extends Component {
    static displayName = Users.name;

    constructor(props) {
        super(props);
        this.state = {users: []};
        this.getUsers = this.getUsers.bind(this);
    }

    async getUsers() {
        await fetch("../api/users", {
            method: "GET"
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    return response.text().then(e => { throw new Error(e.message) });
                }
            })
            .then(data => {
                this.setState({users: data});
            })
            .catch(error => console.log(error));
    }

    render() {
        return <div>
            {
                // this.state.users.map(user => {
                //     return <p>{ user }</p>;
                // })
            }

            <button onClick={this.getUsers}>Get users</button>
        </div>;
    }
}