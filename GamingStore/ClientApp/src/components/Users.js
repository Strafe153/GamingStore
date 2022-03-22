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
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .then(data => {
                this.setState({users: data});
            })
            .catch(error => console.log(error.message));
    }

    render() {
        return <div>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Id</th>
                        <th className="text-center">Username</th>
                        <th className="text-center">Role</th>
                    </tr>
                </thead>
                <tbody>
                {
                    this.state.users.map(user => {
                        return <tr key={user.id}>
                            <td>{ user.id }</td>
                            <td>{ user.username }</td>
                            <td>{ user.role }</td>
                        </tr>;
                    })
                }
                </tbody>
            </table>

            <button onClick={this.getUsers}>Get users</button>
        </div>;
    }
}