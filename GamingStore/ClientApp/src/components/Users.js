import React, { Component } from 'react';
import { UserRow } from './UserRow';
import { formErrorMessage } from '../modules/errorMessageFormer';

export class Users extends Component {
    static displayName = Users.name;

    constructor(props) {
        super(props);

        this.state = {
            users: [],
            role: sessionStorage.getItem('role'),
            token: sessionStorage.getItem('token')
        };

        this.getUsers = this.getUsers.bind(this);
    }

    async componentDidMount() {
        await this.getUsers();
    }

    async getUsers() {
        await fetch('../api/users', {
            method: 'GET'
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                
                return formErrorMessage(response);
            })
            .then(data => this.setState({users: data}))
            .catch(error => console.log(error.message));
    }

    handleUsers = users => {
        this.setState({users: users});
    }

    render() {
        return <div>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Profile Picture</th>
                        <th className="text-center">Id</th>
                        <th className="text-center">Username</th>
                        <th className="text-center">Role</th>
                        <th className="text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                {
                    this.state.users.map(user => {
                        return <UserRow onGetUsers={this.handleUsers}
                                        key={user.id}
                                        profilePicture={user.profilePicture}
                                        id={user.id}
                                        username={user.username}
                                        role={user.role}
                                        calledFromAdmin={this.state.role === '0'}
                                        getUsers={this.getUsers} />
                    })
                }
                </tbody>
            </table>
        </div>;
    }
}